using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DBAccess
{
    public class DbAccess
    {
        public async Task<List<Audio>> GetAllAudio()
        {
            using var dbContext = new DBContext();
            return await dbContext.Audio.ToListAsync();
        }

        public async Task<Audio> GetOneAudioName(string name)
        {
            using var dbContext = new DBContext();
            var audio = await dbContext.Audio.Include(a => a.Category).Include(a => a.Genre).Include(a => a.Type).Include(a => a.Loudness).Include(a => a.Instrument).Include(a => a.Mood).Include(a => a.Other).Include(a => a.UsedIn).Include(a => a.MadeOf).FirstOrDefaultAsync(x => x.Name == name);
            if (audio != null)
            {
                return audio;
            }
            return new Audio();
        }

        public async Task<Audio> GetOneAudioID(int id)
        {
            using var dbContext = new DBContext();
            var audio = await dbContext.Audio.Include(a => a.Category).Include(a => a.Genre).Include(a => a.Type).Include(a => a.Loudness).Include(a => a.Instrument).Include(a => a.Mood).Include(a => a.Other).Include(a => a.UsedIn).Include(a => a.MadeOf).FirstOrDefaultAsync(x => x.ID == id);
            if (audio != null)
            {
                return audio;
            }
            return new Audio();
        }

        public async Task<bool> AddNewAudio(Audio audio)
        {
            using var dbContext = new DBContext();

            try
            {
                dbContext.Add(audio);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> EditAudio(Audio audio, int id)
        {
            using var dbContext = new DBContext();
            Audio audioOriginal = await dbContext.Audio.FirstOrDefaultAsync(x => x.ID == id);
            if (audio == null)
            {
                return false;
            }
            try
            {
                audioOriginal = audio;
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<List<Audio>> GetMultipleAudioCategory(List<Category> categorys)
        {
            bool flag = false;
            using var dbContext = new DBContext();
            List<Audio> audioList = await dbContext.Audio.Include(a => a.Category).ToListAsync();
            List<Audio> returnList = new List<Audio>();

            foreach (var audio in audioList)
            {
                foreach (var category in audio.Category)
                {
                    flag = true;
                    foreach (var returnCategory in categorys)
                    {
                        if (category.Name == returnCategory.Name && !returnList.Contains(audio) && flag == true)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    if (flag) { returnList.Add(audio); }
                }
            }

            if (returnList.Count > 0)
            {
                return returnList;
            }

            return new List<Audio>();
        }

        public async Task<List<Audio>> GetMultipleAudioGenre(List<Genre> genres)
        {
            bool flag = false;
            using var dbContext = new DBContext();
            List<Audio> audioList = await dbContext.Audio.Include(a => a.Genre).ToListAsync();
            List<Audio> returnList = new List<Audio>();

            foreach (var audio in audioList)
            {
                foreach (var genre in audio.Genre)
                {
                    flag = true;
                    foreach (var returnGenre in genres)
                    {
                        if (genre.Name == returnGenre.Name && !returnList.Contains(audio) && flag == true)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    if (flag) { returnList.Add(audio); }
                            
                }
            }

            if (returnList.Count > 0)
            {
                return returnList;
            }

            return new List<Audio>();
        }

        public async Task<List<Category>> GetCategory()
        {
            using var dbContext = new DBContext();
            return await dbContext.Category.ToListAsync();
        }

        public async Task<List<Genre>> GetGenre()
        {
            using var dbContext = new DBContext();
            return await dbContext.Genre.ToListAsync();
        }

        public async Task<bool> AddNewCategory(Category category)
        {
            using var dbContext = new DBContext();

            var existingCategorys = await dbContext.Category.ToListAsync();
            try
            {
                if (!existingCategorys.Contains(category))
                {
                    existingCategorys.Add(category);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> AddNewGenre(Genre genre)
        {
            using var dbContext = new DBContext();

            var existingGenres = await dbContext.Genre.ToListAsync();
            try
            {
                if (!existingGenres.Contains(genre))
                {
                    existingGenres.Add(genre);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
