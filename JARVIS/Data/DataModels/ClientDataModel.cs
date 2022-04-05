using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Jarvis.Data.DataModels
{
    public class ClientDataModel : FiscalEntityDataModel
    {
        public override string ShortDescriptor => ToString();

        private DateTime? birthDate;
        /// <summary>
        /// The birthdate of the person, as string
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
                    var birthDateInDateFormat = DateTime.Parse( value ).Date;

                    SetProperty( ref birthDate , birthDateInDateFormat );

                    // Save today's date.
                    var today = DateTime.Today;

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
            private set
            {
                SetProperty( ref age , value );

                if ( age < 18 )
                {
                    IsDependent = true;
                }
                else
                {
                    IsDependent = null;
                }
            }
        }

        private bool? isDependent;
        public bool? IsDependent
        {
            get => isDependent;
            set => SetProperty( ref isDependent , value );
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

        private int? aggregateId;
        public virtual int? AggregateId
        {
            get => aggregateId;
            set => SetProperty( ref aggregateId , value );
        }


        private AggregateDataModel aggregate;
        public virtual AggregateDataModel Aggregate
        {
            get => aggregate;
            set => SetProperty( ref aggregate , value );
        }

        public override string ToString()
        {
            var baseDescriptor = base.ToString();
            var description = new StringBuilder( baseDescriptor );
            description.AppendLine( $"Data Nasc.: {BirthDate}" );
            return description.ToString();

        }

    }
}
