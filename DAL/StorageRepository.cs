using Storage.Domain;

namespace Storage.DAL
{
    internal class StorageRepository
    {
        private readonly StorageDbContext _context;

        public StorageRepository(StorageDbContext context)
        {
            _context = context;
        }

        public int AddPallet(Pallet pallet)
        {
            if (pallet == null)
                throw new ArgumentNullException("Received an empty pallet");

            _context.Pallets.Add(pallet);
            _context.SaveChanges();

            return pallet.Id;
        }

        public int AddBox(Box box, Pallet pallet)
        {
            if (box == null)
                throw new ArgumentNullException("Received an empty box");

            pallet.AddBox(box);
            _context.Boxes.Add(box);
            _context.SaveChanges();

            return box.Id;
        }

        public List<Pallet> GetPallets()
        {    
            var boxes = _context.Boxes.ToList();
            var pallets = _context.Pallets.ToList();

            return pallets;
        }

        public List<Box> GetBoxes()
        {
            return _context.Boxes.ToList();
        }

        public Dictionary<DateTime, List<Pallet>> GetGroups()
        {
            var dateToPallet = new Dictionary<DateTime, List<Pallet>>();
            var groups = GetPallets().OrderBy(p => p.ExpirationDate).GroupBy(p => p.ExpirationDate);

            foreach (var group in groups)
            {
                dateToPallet.Add(group.Key, group.OrderBy(p => p.Volume).ToList());
            }

            return dateToPallet;
        }

        public Pallet[] GetPalletsWithHigherBoxExpirationDate()
        {
            var pallets = GetPallets();
            if (pallets.Count() <= 3)
                return pallets.OrderBy(p => p.Volume).ToArray();

            var sortedPallets = new Pallet[3];

            var i = 0;

            
            foreach (var box in GetBoxes().OrderByDescending(b => b.ExpirationDate).DistinctBy(b => b.PalletId))
            {
                if (i == 3)
                    break; 

                sortedPallets[i] = pallets.FirstOrDefault(p => p.Id == box.PalletId);
                i++;
            }

            return sortedPallets.OrderBy(p => p.Volume).ToArray();
        }
    }
}
