using DBAccess;
using Models;

namespace BusinessLogic
{
    public class BLogic
    {
        private DbAccess dbAccess;
        public BLogic()
        {
            dbAccess = new DbAccess();
        }

        /// <summary>
        /// Gives all the data we have saved in our db
        /// </summary>
        /// <returns>(List<Audio>) gets all audio in a list</returns>
        public async Task<List<Audio>> GetAllAudio() { return await dbAccess.GetAllAudio(); }

        /// <summary>
        /// Giver et specefik data sæt ud fra navnet på lyden
        /// </summary>
        /// <param name="name">(string)</param>
        /// <returns>(audio)Custom object</returns>
        public async Task<Audio> GetOneAudioName(string name) { return await dbAccess.GetOneAudioName(name); }

        /// <summary>
        /// Giver et specefik data sæt ud fra id på lyden
        /// </summary>
        /// <param name="id">(int)</param>
        /// <returns>(audio)Custom object</returns>
        public async Task<Audio> GetOneAudioID(int id) { return await dbAccess.GetOneAudioID(id); }

        /// <summary>
        /// Tilføjer en ny lyd
        /// </summary>
        /// <param name="audio">(audio) custom object</param>
        /// <returns>(bool)</returns>
        public async Task<bool> AddNewAudio(Audio audio) {
            AudioMetaDataLink audioMetaDataLink = new AudioMetaDataLink();
            audio = await audioMetaDataLink.AudioExtract(audio);
            return await dbAccess.AddNewAudio(audio); 
        }

        /// <summary>
        /// Henter alle lyde der passer med alle kategorierne som er blevet modtaget
        /// </summary>
        /// <param name="categorys">(List<Category>)</param>
        /// <returns>(List<Audio>)</returns>
        public async Task<List<Audio>> GetMultipleAudioCategory(List<Category> categorys) { return await dbAccess.GetMultipleAudioCategory(categorys); }

        /// <summary>
        /// Henter alle lyde der passer med alle genre som er blevet modtaget
        /// </summary>
        /// <param name="genres">(List<Genre>)</param>
        /// <returns>(List<Audio>)</returns>
        public async Task<List<Audio>> GetMultipleAudioGenre(List<Genre> genres) { return await dbAccess.GetMultipleAudioGenre(genres); }

        /// <summary>
        /// Henter alle kategorier som er blevet gemt
        /// </summary>
        /// <returns>(List<Category>)</returns>
        public async Task<List<Category>> GetCategory() { return await dbAccess.GetCategory(); }

        /// <summary>
        /// Tilføjer en ny kategori
        /// </summary>
        /// <param name="category">den nye kategori</param>
        /// <returns>(bool)</returns>
        public async Task<bool> AddNewCategory(Category category) { return await dbAccess.AddNewCategory(category); }

        /// <summary>
        /// Henter alle genre som er blevet gemt
        /// </summary>
        /// <returns>(List<Genre>)</returns>
        public async Task<List<Genre>> GetGenre() { return await dbAccess.GetGenre(); }

        /// <summary>
        /// Tilføjer en ny genre
        /// </summary>
        /// <param name="genre">den nye genre</param>
        /// <returns>(bool)</returns>
        public async Task<bool> AddNewGenre(Genre genre) { return await dbAccess.AddNewGenre(genre); }
    }
}
