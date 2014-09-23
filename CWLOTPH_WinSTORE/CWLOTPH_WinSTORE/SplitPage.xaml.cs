using CWLOTPH_WinSTORE.Common;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента страницы с разделением задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234234
using CWLOTPH_WinSTORE.DataModel;

namespace CWLOTPH_WinSTORE
{
    /// <summary>
    /// Страница, на которой отображается заголовок группы, список элементов этой группы и сведения о
    /// выбранном в данный момент элементе.
    /// </summary>
    public sealed partial class SplitPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper используется на каждой странице для облегчения навигации и 
        /// управление жизненным циклом процесса
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Эту настройку можно изменить на модель строго типизированных представлений.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public SplitPage()
        {
            this.InitializeComponent();

            // Настройка вспомогательного приложения навигации
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            // Настройка логических компонентов навигации по страницам, позволяющих
            // чтобы на странице отображалась только одна плитка.
            this.navigationHelper.GoBackCommand = new RelayCommand(this.GoBack, this.CanGoBack);
            this.itemListView.SelectionChanged += ItemListView_SelectionChanged;

            // Начало прослушивания изменений размера окна 
            // чтобы перейти от отображения двух панелей к отображению одной
            Window.Current.SizeChanged += Window_SizeChanged;
            this.InvalidateVisualState();

            this.Unloaded += SplitPage_Unloaded;
        }

        /// <summary>
        /// Удалить обработчик события SizedChanged при выгрузке SplitPage.
        /// </summary>
        private void SplitPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации.  Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы и
        /// словарь состояний, сохраненных этой страницей в ходе предыдущего
        /// сеанса.  Это состояние будет равно NULL при первом посещении страницы.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var selectedItem = VisualisationDataObject.VectorObject.FirstOrDefault(x => x.UniqueId.Equals((String)e.NavigationParameter));
            itemListView.ItemsSource = VisualisationDataObject.VectorObject;

            if (selectedItem != null)
            {
                if (itemListView.Items != null)
                    itemListView.SelectedItem = itemListView.Items.FirstOrDefault(x =>
                    {
                        var vectorObject = x as VectorObject;
                        return vectorObject != null && selectedItem.UniqueId.Equals(vectorObject.UniqueId);
                    });
            }
            else
            {
                if (itemListView.Items != null && itemListView.Items.Count > 0) itemListView.SelectedItem = itemListView.Items[0];
            }
        }

        /// <summary>
        /// Сохраняет состояние, связанное с данной страницей, в случае приостановки приложения или
        /// удаления страницы из кэша навигации.  Значения должны соответствовать требованиям сериализации
        /// <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="navigationParameter">Значение параметра, передаваемое
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы.
        /// </param>
        /// <param name="sender">Источник события; как правило, <see cref="NavigationHelper"/></param>
        /// <param name="e">Данные события, которые предоставляют пустой словарь для заполнения
        /// сериализуемым состоянием.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

        }

        #region Логическая навигация по страницам

        // Разделенная страница разработана таким образом, чтобы в случае, когда в окне недостаточно места для отображения
        // как списка, так и сведений, отображалась только одна из панелей.
        //
        // Все это реализуется с помощью единственной физической страницы, которая может представлять две логические
        // страницы.  В следующем коде эта цель достигается без необходимости ставить пользователя в известность о
        // разделении.

        private const int MinimumWidthForSupportingTwoPanes = 768;

        /// <summary>
        /// Вызывается, чтобы определить, должна страница использоваться в качестве одной логической страницы или в качестве двух.
        /// </summary>
        /// <returns>Значение True, если окно должно вести себя как одна логическая страница, значение False
        /// в противном случае.</returns>
        private bool UsingLogicalPageNavigation()
        {
            return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes;
        }

        /// <summary>
        /// Вызывается при изменении размера окна
        /// </summary>
        /// <param name="sender">Текущее окно</param>
        /// <param name="e">Данные о событии, описывающие новый размер окна</param>
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Вызывается при выделении элемента списка.
        /// </summary>
        /// <param name="sender">Представление сетки, в котором отображается выделенный элемент.</param>
        /// <param name="e">Данные о событии, описывающие, каким образом изменилось выделение.</param>
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Аннулировать состояние представления, когда действует логическая навигация по страницам, поскольку изменение
            // выделения может вызвать соответствующее изменение в текущей логической странице.  Если
            // элемент выделен, это приводит к переходу от отображения списка элементов к
            // отображению сведений о выделенном элементе.  Когда выделение очищается, это приводит к
            // обратному эффекту.

            if (e.AddedItems.Count>0)
            {
                var vectorItem = e.AddedItems[0] as VectorObject;
                VectorItemsListView.ItemsSource = VisualisationDataObject.VectorObject.ElementAt(
                    VisualisationDataObject.VectorObject.IndexOf(
                        vectorItem)).Items;
            }
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }

        private void CalculateButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ResultPage), null);
        }

        private bool CanGoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                return true;
            }
            else
            {
                return this.navigationHelper.CanGoBack();
            }
        }
        private void GoBack()
        {
            if (this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null)
            {
                // Если действует логическая навигация по страницам и выделен элемент,
                // сведения о котором в данный момент отображаются.  При очистке выделения снова отображается
                // список элементов.  С точки зрения пользователя это логический переход назад
                // назад.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                this.navigationHelper.GoBack();
            }
        }

        private void InvalidateVisualState()
        {
            var visualState = DetermineVisualState();
            VisualStateManager.GoToState(this, visualState, false);
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Вызывается, чтобы определить имя состояния отображения, соответствующее состоянию
        /// отображения приложения.
        /// </summary>
        /// <returns>Имя требуемого состояния отображения.  Это имя совпадает с именем состояния
        /// отображения, кроме случаев, когда есть выделенный элемент в книжном или прикрепленном представлении, где
        /// эта дополнительная логическая страница представляется добавлением суффикса _Detail.</returns>
        private string DetermineVisualState()
        {
            if (!UsingLogicalPageNavigation())
                return "PrimaryView";

            // Обновить включенное состояние кнопки "Назад" при изменении состояния представления
            var logicalPageBack = this.UsingLogicalPageNavigation() && this.itemListView.SelectedItem != null;

            return logicalPageBack ? "SinglePane_Detail" : "SinglePane";
        }

        #endregion

        #region Регистрация NavigationHelper

        /// Методы, предоставленные в этом разделе, используются исключительно для того, чтобы
        /// NavigationHelper для отклика на методы навигации страницы.
        /// 
        /// Логика страницы должна быть размещена в обработчиках событий для 
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// и <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// Параметр навигации доступен в методе LoadState 
        /// в дополнение к состоянию страницы, сохраненному в ходе предыдущего сеанса.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}