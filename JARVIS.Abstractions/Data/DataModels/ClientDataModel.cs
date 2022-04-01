using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Jarvis.DataModels
{
    public class ClientDataModel : FiscalEntityDataModel
    {
        public override string ShortDescriptor => ToString();

        private DateTime? birthDate;
        /// <summary>
        /// The ClientModel birthdate, as string
        /// </summary>
        public string BirthDate
        {
            get
            {
                if ( birthDate.HasValue )
                {
                    return birthDate.Value.Date.ToShortDateString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if ( !string.IsNullOrWhiteSpace( value ) )
                {
                    //Convert the supplied string to date
                    DateTime birthDateInDateFormat = DateTime.Parse( value ).Date;

                    SetProperty( ref birthDate , birthDateInDateFormat );

                    // Save today's date.
                    DateTime today = DateTime.Today;

                    // Calculate the age.
                    int? age = today.Year - birthDateInDateFormat.Year;

                    // Go back to the year the person was born in case of a leap year
                    if ( birthDateInDateFormat.Date > today.AddYears( -age.Value ) )
                    {
                        age--;
                    }

                    Age = age;
                }
            }
        }

        [NotMapped]
        private int? age;

        [NotMapped]
        public int? Age
        {
            get => age;
            private set => SetProperty( ref age , value );
        }

        private string gender;
        /// <summary>
        /// The client gender
        /// </summary>
        public string Gender
        {
            get => gender;
            set => SetProperty( ref gender , value );
        }

        private string nationality;
        /// <summary>
        /// The client nationality
        /// </summary>
        public string Nationality
        {
            get => nationality;
            set => SetProperty( ref nationality , value );
        }

        public override string ToString()
        {
            string baseDescriptor = base.ToString();
            StringBuilder description = new StringBuilder( baseDescriptor );
            description.AppendLine();
            description.AppendLine( $"Género: {Gender}" );
            description.AppendLine( $"Data Nasc.: {BirthDate}" );
            description.AppendLine( $"Nacionalidade: {Nationality}" );
            return description.ToString();

        }
    }
}
