using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Jarvis.Controllers.ModelControllers.Factories;
using Jarvis.Controllers.ScreenControllers;
using Jarvis.Data.Contract;
using Jarvis.Data.Contract.Repositories;
using Jarvis.Data.DataAccess.Repositories;
using Jarvis.Data.DataModels;
using Jarvis.Services;
using Jarvis.Utils.HelperClasses;
using Microsoft.VisualBasic.FileIO;

namespace Jarvis.Controllers.ModelControllers
{
    public class CSVImportHandler
    {
        public static List<(string, string)> EntitiesToProcess;

        internal static List<ProcessingResult> ProcessImport( string pathToCsv , BackgroundWorker worker )
        {
            var processingResults = new List<ProcessingResult>();

            try
            {
                var entitiesToProcess = new List<IFiscalEntity>();

                var fileErrors = new List<string>();

                worker?.ReportProgress( -1 , $"A processar o ficheiro indicado" );

                using ( var csvParser = new TextFieldParser( pathToCsv ) )
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters( new string[] { "," } );
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    while ( !csvParser.EndOfData )
                    {
                        string[] fields;
                        try
                        {
                            // Read current line fields, pointer moves to the next line.
                            fields = csvParser.ReadFields();
                        }
                        catch ( MalformedLineException )
                        {
                            fileErrors.Add( $"Linha #{csvParser.ErrorLineNumber} => {csvParser.ErrorLine} | erro ao ler a informação" );
                            continue;
                        }

                        //Check if read fields are ok
                        if ( fields.Length != 2 )
                        {
                            fileErrors.Add( $"Linha #{csvParser.LineNumber} - número de campos disponibilizados diferente de 2" );
                            continue;
                        }

                        var fiscalNumber = fields[0];
                        if ( string.IsNullOrEmpty( fiscalNumber ) )
                        {
                            fileErrors.Add( $"Linha #{csvParser.LineNumber} - Campo de N.I.F. está vazio" );
                            continue;
                        }


                        var password = fields[1];
                        if ( string.IsNullOrEmpty( password ) )
                        {
                            fileErrors.Add( $"Linha #{csvParser.LineNumber} - Campo de password está vazio" );
                            continue;
                        }

                        //Generate entity
                        var errorType = FiscalEntityController.ResolveFiscalInfoToEntity( fiscalNumber , password , out var generatedEntity );

                        if ( errorType == FiscalEntityGenerationResult.InvalidFiscalNumber )
                        {
                            fileErrors.Add( $"Linha #{csvParser.LineNumber} - Número fiscal é invalido" );
                            continue;
                        }
                        else if ( errorType == FiscalEntityGenerationResult.CannotResolveFiscalNumberToEntityType )
                        {
                            fileErrors.Add( $"Linha #{csvParser.LineNumber} - Não foi possível determinar o tipo da entidade" );
                            continue;
                        }

                        entitiesToProcess.Add( generatedEntity );

                    }

                    if ( fileErrors.Count != 0 )
                    {
                        var errorMessage = new StringBuilder();
                        errorMessage.AppendLine( "Erros na importação do ficheiro disponibilizado:" );
                        errorMessage.AppendLine();
                        foreach ( var error in fileErrors )
                        {
                            errorMessage.AppendLine( $"  {error}" );
                        }
                        errorMessage.AppendLine();
                        errorMessage.AppendLine( "Esta linhas serão ignoradas" );
                        WindowService.DisplayMessage( MessageType.Error , errorMessage.ToString() , "Erro na importação do ficheiro" );
                    }
                }

                var counter = 0;
                var totalToProcess = entitiesToProcess.Count;

                using ( IUnitOfWork unitForImport = new UnitOfWork() )
                {
                    foreach ( FiscalEntityDataModel entityToProcess in entitiesToProcess )
                    {
                        var percentage = ModuleUtils.CalculatePercentage( ++counter , totalToProcess);

                        //report progress here, not on the controller , for total loading percentage
                        worker?.ReportProgress( percentage , $"A processar entidade {counter}/{totalToProcess}" );

                        //Get controller for generated entity
                        //update is disabled for the this operation
                        var controller = DataModelControllerFactory.GetControllerForEntity( entityToProcess , worker, unitForImport );

                        //Get all info of entity
                        var result = controller.UpdateEntityInfo(true);

                        //Add processment result
                        processingResults.Add( new ProcessingResult( entityToProcess , result ) );
                    }

                    unitForImport.Complete();
                }
            }
            catch ( Exception ex)
            {
                WindowService.ShowException( ex );
                return null;
            }

            return processingResults;
        }
    }
}
