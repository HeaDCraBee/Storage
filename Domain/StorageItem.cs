using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Domain
{
    internal abstract class StorageItem
    {
        protected readonly int _id;
        protected float _width;
        protected float _height;
        protected float _depth;
        protected DateTime _expirationDate = new DateTime().Date;

        public int Id
        {
            get { return _id; }
        }
        public float Width
        {
            get { return _width; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Width must be above zero");

                _width = value;
            }
        }
        public float Height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Height must be above zero");

                _height = value;
            }
        }
        public float Depth
        {
            get { return _depth; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Depth must be above zero");

                _depth = value;
            }
        }

        public float Volume
        {
            get { return CalculateVolume(); }
        }

        [Column(TypeName = "date")]
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value.Date; }
        }

        public StorageItem(float width, float height, float depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public abstract string ToString(string delimiter);

         protected abstract float CalculateVolume();
    }
}
