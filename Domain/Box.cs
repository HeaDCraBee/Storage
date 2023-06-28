using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Storage.Domain
{
    [PrimaryKey(nameof(Id))]
    internal class Box : StorageItem
    {
        private int _palletId;

        public int PalletId
        {
            get { return _palletId; }
            set {
                if (value < 0)
                    throw new ArgumentException("Pallet id cannot be less than zero");

                _palletId = value;
            }
        }

        [Column(TypeName = "date")]
        public DateTime DateOfManufacture { get; set; } = new DateTime().Date;

        public Box(float width, float height, float depth, DateTime dateOfManufacture) : base(width, height, depth)
        {
            DateOfManufacture = dateOfManufacture.Date;
            ExpirationDate = dateOfManufacture.AddDays(100).Date;
        }

        public Box(float width, float height, float depth, DateTime dateOfManufacture, DateTime expirationDate) : this(width, height, depth, dateOfManufacture)
        {
            ExpirationDate = expirationDate.Date;
        }
        
        protected override float CalculateVolume()
        {
            return _width * _height * _depth;
        }

        public override string ToString()
        {
            return ToString("");
        }

        public override string ToString(string delimiter)
        {
            var sb = new StringBuilder($"{delimiter}Box Id: {Id}\n");
            sb.AppendLine($"{delimiter}\tWidth: {Width}");
            sb.AppendLine($"{delimiter}\tHeight: {Height}");
            sb.AppendLine($"{delimiter}\tDepth: {Depth}");
            sb.AppendLine($"{delimiter}\tDate of manufacture: {DateOfManufacture.ToShortDateString()}");
            sb.AppendLine($"{delimiter}\tExpiration date: {ExpirationDate.ToShortDateString()}");

            return sb.ToString();
        }
    }
}
