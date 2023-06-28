using Storage.DAL;
using Storage.Domain;

namespace Storage.Controller
{
    internal class StorageController
    {
        private readonly StorageRepository _storageRepository;

        public StorageController(StorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public int AddPallet(Pallet pallet)
        {
            return _storageRepository.AddPallet(pallet);
        }

        public int AddBox(Box box, Pallet pallet)
        {
            return _storageRepository.AddBox(box, pallet);
        }

        public List<Pallet> GetPallets()
        {
            return _storageRepository.GetPallets();
        }

        public List<Box> GetBoxes()
        {
            return _storageRepository.GetBoxes();
        }

        public Dictionary<DateTime, List<Pallet>> GetGroups()
        {
            return _storageRepository.GetGroups();
        }

        public Pallet[] GetPalletsWithHigherBoxExpirationDate()
        {
            return _storageRepository.GetPalletsWithHigherBoxExpirationDate();
        }
    }
}
