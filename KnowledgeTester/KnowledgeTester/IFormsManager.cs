using System.Windows;

namespace KnowledgeTester
{
    /// <summary>
    /// Менеджер форм 
    /// </summary>
    public interface IFormsManager
    {
        /// <summary>
        /// Показать форму
        /// </summary>
        /// <param name="viewName">Название формы для получения из IoC контейнера</param>
        void Show(string viewName);

        /// <summary>
        /// Показать форму
        /// </summary>
        /// <param name="viewName">Название формы для получения из IoC контейнера</param>
        /// <param name="id">Идентификатор отображаемых данных (одна форма новые данные открывает в новом окне)</param>
        /// <param name="data">Данные для формы</param>
        /// <param name="owner">Владелец формы, при закрытии которой должна закрыться и эта форма</param>
        void Show(string viewName, object id, object data, Window owner);

        /// <summary>
        /// Показать форму как модальное окно
        /// </summary>
        /// <param name="viewName">Название формы для получения из IoC контейнера</param>
        /// <param name="data">Данные для формы</param>
        /// <param name="owner">Форма, которая вызвала модальное окно</param>
        /// <returns>Результат отображения модального окна</returns>
        object ShowModal(string viewName, object data, Window owner);
    }
}
