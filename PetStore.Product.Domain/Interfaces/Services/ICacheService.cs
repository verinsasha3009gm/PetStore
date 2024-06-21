using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Interfaces.Services
{
    public interface ICacheService
    {
        /// <summary>
        /// метод считывания обьекта из кеша по ключу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// метод записи обьекта в кеш 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        T Set<T>(string key, T value);
        /// <summary>
        /// метод удаления обьекта из кеша по ключу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Delete<T>(string key);
        /// <summary>
        /// метод обновления обьекта в кеше по ключу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Refrech<T>(string key);
    }
}
