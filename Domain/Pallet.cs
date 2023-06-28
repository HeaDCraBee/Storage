using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Storage.Domain
{
    [PrimaryKey(nameof(Id))]
    internal class Pallet : StorageItem
    {
        public LinkedList<Box> Boxes { get; set; }

        public Pallet(float width, float height, float depth) : base(width, height, depth)
        {
            Boxes = new LinkedList<Box>();
            ExpirationDate = default;
        }

        public Pallet(float width, float height, float depth, LinkedList<Box> boxes) : base(width, height, depth)
        {
            if (boxes == null || boxes.Count == 0)
            {
                Boxes = new LinkedList<Box>();
                ExpirationDate = default;
                return;
            }

            if (boxes.ToHashSet().Count != boxes.Count)
                throw new ArgumentException("Boxes ids must be unique");

            Boxes = new LinkedList<Box>(boxes.OrderBy(b => b.ExpirationDate));
            ExpirationDate = Boxes.First.Value.ExpirationDate;
        }

        public void AddBox(Box newBox)
        {
            if (Boxes.FirstOrDefault(b => b.Id == newBox.Id) != null)
                throw new ArgumentException("Box with current id already in this pallet");

            if (newBox.Width > Width || newBox.Depth > Depth)
                throw new ArgumentException("Box can not be biger than pallet");

            //При добавлении коробки помещаем ее в соответствии со сроком годности
            var current = Boxes.First;

            if (current == null || newBox.ExpirationDate < current.Value.ExpirationDate)
            {
                ExpirationDate = newBox.ExpirationDate;
                Boxes.AddFirst(newBox);
                return;
            }

            current = current.Next;

            while (current != null)
            {
                if (newBox.ExpirationDate < current.Value.ExpirationDate)
                {
                    Boxes.AddBefore(current, newBox);
                    return;
                }

                current = current.Next;
            }

            newBox.PalletId = Id;

            Boxes.AddLast(newBox);
        }

        public override string ToString()
        {
            return ToString("");
        }

        public override string ToString(string delimiter)
        {
            var sb = new StringBuilder($"{delimiter}Pallet Id{Id}:\n");
            sb.AppendLine($"{delimiter}\tWidth: {Width}");
            sb.AppendLine($"{delimiter}\tHeight: {Height}");
            sb.AppendLine($"{delimiter}\tDepth: {Depth}");
            sb.AppendLine($"{delimiter}\tVolume: {Volume}");
            if (Boxes.Count != 0)
            {
                sb.AppendLine($"{delimiter}\tExpiration date: {ExpirationDate.ToShortDateString()}");
                sb.AppendLine($"{delimiter}\tBoxes:");
                foreach (var box in Boxes)
                {
                    sb.AppendLine(box.ToString(delimiter + "\t"));
                }
            }

            return sb.ToString();
        }

        protected override float CalculateVolume()
        {
            float volume = 0;

            foreach (Box box in Boxes)
            {
                volume += box.Volume;
            }

            return volume + _width * _height * _depth;
        }
    }
}
