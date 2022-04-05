using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Jarvis.Data.DataModels
{
    public class VehiecleDataModel : BaseDataModel
    {
        /// <summary>
        /// The common/human-friendly ID for vehiecles
        /// </summary>
        public override string CommonId => LicensePlate;

        public override string ShortDescriptor => ToString();

        private FiscalEntityDataModel owner;
        /// <summary>
        /// A reference to the owner of the Vehiecle
        /// </summary>
        public FiscalEntityDataModel Owner
        {
            get => owner;
            set => SetProperty( ref owner , value );
        }

        private int ownerId;
        /// <summary>
        /// A reference to the owner of the Vehiecle
        /// </summary>
        public int OwnerId
        {
            get => ownerId;
            set => SetProperty( ref ownerId , value );
        }



        private string roleOfClient;
        /// <summary>
        /// The role the ClientModel has wiht vehiecle (owner, renter, etc.)
        /// </summary>
        public string RoleOfClient
        {
            get => roleOfClient;
            set => SetProperty( ref roleOfClient , value );
        }

        private string licensePlate;
        /// <summary>
        /// The license plate of the vehiecle
        /// </summary>
        public string LicensePlate
        {
            get => licensePlate;
            set => SetProperty( ref licensePlate , value );
        }

        private DateTime? dateOfLicensePlate;
        /// <summary>
        /// The timestamp that the license was emited in
        /// </summary>
        public DateTime? DateOfLicensePlate
        {
            get => dateOfLicensePlate;
            set => SetProperty( ref dateOfLicensePlate , value );
        }

        [NotMapped]
        public string DateOfLicensePlateString => DateOfLicensePlate?.ToString( "y" );

        private string brand;
        /// <summary>
        /// The vehiecle brand
        /// </summary>
        public string Brand
        {
            get => brand;
            set => SetProperty( ref brand , value );
        }


        private string model;
        /// <summary>
        /// The model of the Vehiecle
        /// </summary>
        public string Model
        {
            get => model;
            set => SetProperty( ref model , value );
        }

        private string notes;
        /// <summary>
        /// Notes regarding this vehiecle
        /// </summary>
        public string Notes
        {
            get => notes;
            set => SetProperty( ref notes , value );
        }

        public override string ToString()
        {
            var vehiecleDescription = new StringBuilder();
            vehiecleDescription.AppendLine( $"Matrícula: {LicensePlate}" );
            vehiecleDescription.AppendLine( $"Marca: {Brand}" );
            vehiecleDescription.AppendLine( $"Modelo: {Model}" );
            vehiecleDescription.AppendLine( $"Registado em: {DateOfLicensePlate?.ToString( "d" )}" );
            vehiecleDescription.Append( $"Papel do Cliente: {RoleOfClient}" );
            return vehiecleDescription.ToString();

        }
    }

}
