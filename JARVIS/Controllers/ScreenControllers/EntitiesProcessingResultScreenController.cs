using System.Collections.Generic;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;

using Jarvis.Interfaces;
using Jarvis.Services;

namespace Jarvis.Controllers.ScreenControllers
{
    public class EntitiesProcessingResultScreenController : PropertyRaiser, IDismissable
    {
        private List<ProcessingResult> entitiesProcessingStatus;
        public List<ProcessingResult> EntitiesProcessingStatus
        {
            get => entitiesProcessingStatus;
            set => SetProperty( ref entitiesProcessingStatus , value );
        }

        public ICommand OkCommand
        {
            get;
            set;
        }

        public ICommand CancelCommand
        {
            get;
            set;
        }

        public bool DisplayControlButtons
        {
            get;
            set;
        }

        public EntitiesProcessingResultScreenController()
        {
            DisplayControlButtons = true;
            OkCommand = new RelayCommand( OkAction );
            CancelCommand = new RelayCommand( CancelAction );
        }

        private void OkAction()
        {
            WindowService.CloseWindowOfViewModel( this , true );
        }

        private void CancelAction()
        {
            WindowService.CloseWindowOfViewModel( this , false );
        }
    }

    public class ProcessingResult
    {
        public string FiscalNumber => Entity?.FiscalNumber;

        public string Name => Entity?.Name;

        public string ResultAsString
        {
            get
            {
                switch ( Result )
                {
                    case OperationResult.Success:
                        return "Sim";
                    case OperationResult.WrongCredentials:
                        return "NIF/Password Errada";
                    case OperationResult.Failed:
                        return "Não";
                    case OperationResult.Default:
                    default:
                        return "Desconhecido";
                }
            }
        }

        public IFiscalEntity Entity
        {
            get;
            private set;
        }

        public OperationResult Result
        {
            get;
            private set;
        }

        public ProcessingResult( IFiscalEntity entity , OperationResult result )
        {
            Entity = entity;
            Result = result;
        }
    }

}