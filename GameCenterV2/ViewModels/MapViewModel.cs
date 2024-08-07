using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameCenterV2.Models;
using GameCenterV2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GameCenterV2.Views;
using System.Security.AccessControl;
using Microsoft.Maui.Controls;

namespace GameCenterV2.ViewModels
{
    public partial class MapViewModel : BaseViewModel
    {
        private readonly MapServices _mapService = new MapServices ();
        [ObservableProperty] ObservableCollection<TableShow?> crossTables = new ObservableCollection<TableShow?>();
        
        
        bool isFirstRun;
       
        public MapViewModel(MapServices mapService)
        {
            _mapService = mapService;
            isFirstRun = true;
            //CrossTables = new ObservableCollection<TableShow>();
        }
        public MapViewModel()
        {

        }
        [ObservableProperty]
        public bool isUserLoggedIn;
       
        [RelayCommand]
        public async Task Init()
        {
            //await CheckOpenBox();
            await GetTableAsync();
            
        }
        private async Task GetTableAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var table = await _mapService.GetTableAsync();
                var activeTables = await _mapService.GetActiveTableAsync();
                CrossTables.Clear();
                var mergedTables = new HashSet<int>(); // استخدام HashSet لتحسين الأداء

                if (table != null && table.Any())
                {
                    foreach (var tableItem in table)
                    {
                        bool isActive = false;
                        bool isMerged = false;
                        string? isMerge = null;
                        int? _nnid = null;

                        if (activeTables != null && activeTables.Any())
                        {
                            var activeTable = activeTables.FirstOrDefault(a => a.TbServices.Any(s => s.SeTNumber == tableItem.TId));

                            if (activeTable != null)
                            {
                                isActive = true;
                                _nnid = activeTable.MId;

                                var service = activeTable.TbServices.FirstOrDefault(s => s.SeTNumber == tableItem.TId);
                                if (service != null)
                                {
                                    isMerge = service.SeMargeTo;
                                }
                            }
                        }

                        // تحويل قيمة isMerge إلى قائمة أرقام وتحديث HashSet
                        var mergeNumbers = isMerge?.Split(',').Select(int.Parse);
                        if (mergeNumbers != null)
                        {
                            foreach (var number in mergeNumbers)
                            {
                                mergedTables.Add(number);
                            }
                        }

                        isMerged = mergedTables.Contains(tableItem.TNumber);

                        // إنشاء نموذج عرض الطاولة
                        TableShow tableShow = new TableShow
                        {
                            TbImage = tableItem.TbImage,
                            TDefaultItem = tableItem.TDefaultItem,
                            TDefaultItemNavigation = tableItem.TDefaultItemNavigation,
                            TDetails = tableItem.TDetails,
                            TId = tableItem.TId,
                            TMap = tableItem.TMap,
                            TName = tableItem.TName,
                            TNumber = tableItem.TNumber,
                            IsActive = isActive,
                            IsMarged = isMerged, 
                            Menuid = _nnid,
                        };

                        // إضافة الطاولة إلى القائمة
                        CrossTables.Add(tableShow);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Unable to get customer.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        //private async Task CheckOpenBox()
        //{
        //    BoxServices _box = new BoxServices();
        //    var boxes = await _box.GetBoxAsync();
        //    bool isopen = false;
        //    Int32 UID = 0;
        //    foreach (var box in boxes)
        //    {
        //        if (box != null)
        //        {
        //            isopen = box.BoxIsopen;
        //            if (isopen)
        //            {
        //                UID = box.BoxUId ?? 0;
        //                PublicVariables.BoxID = box.BoxId;
        //                PublicVariables.OpenBox = box.BoxIsopen;

        //            }
        //            break;
        //        }
        //    }
        //    if (!isopen)
        //    {
        //        await Shell.Current.GoToAsync(nameof(OpenBoxPage), true);
        //    }
        //}

        [RelayCommand]
        private async Task TableTappedAsync(object selectedItem)
        {
            if (selectedItem is TableShow tappedTable)
            {
                var viewModel = new PosViewModel()
                {
                    Menuid = tappedTable.Menuid,
                    Tableid = tappedTable.TNumber,
                };
                
                await Shell.Current.GoToAsync(nameof(PosPage), new Dictionary<string, object>
                {
                    { "ViewModel", viewModel }
                });
             }
        }

    }
}
