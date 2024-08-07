using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameCenterV2.Models;
using GameCenterV2.Services;
using GameCenterV2.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameCenterV2.ViewModels
{
    public partial class MergeTablesViewModel : BaseViewModel
    {
        [ObservableProperty] public ObservableCollection<TableShow> tables;
        [ObservableProperty] public int orginalTable;
        [ObservableProperty] public int menuId;
        [ObservableProperty] public bool cancelMerge;
        [ObservableProperty] public bool moveTable;

        public ICommand ConfirmMergeCommand => new RelayCommand(async () => await ConfirmMerge());

        ServiceServices serviceServices = new ServiceServices();
        public MergeTablesViewModel()
        {
            // تحميل بيانات الطاولات
           Tables = new ObservableCollection<TableShow>();
            LoadTables();
        }
        MapServices _mapService = new MapServices();
        private async Task LoadTables()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var table = await _mapService.GetNotActiveTableAsync();
                Tables.Clear();
                if (table != null && table.Any())
                {
                    foreach (var tableItem in table)
                    {
                       
                        //لان انشأت كلاس جديد علمود لا يتخربط بين موديل الجدول وبين موديل عرض الجدول
                        // لان موديل عرض الجدول فيه اضافة وهي اذا كانت فعالة ام لا
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

                        };
                        //الطاولات الفعالة 

                        Tables.Add(tableShow);
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

        private async Task ConfirmMerge()
        {
            // الحصول على الطاولات المختارة
            var selectedTables = Tables.Where(t => t.IsSelected).Select(t => t.TNumber).ToList();

            if (selectedTables.Count > 0)
            {
                
                // تحويل الطاولات المختارة إلى نص مفصول بفاصلة
                var tablesToMerge = string.Join(",", selectedTables);
                bool isok = false;
                if (!CancelMerge && !MoveTable)
                {
                    selectedTables.Add(OrginalTable);
                    // حفظ البيانات باستخدام API
                    var success = await SaveMergedTablesAsync(tablesToMerge);
                    isok = success;
                }
                else if (CancelMerge)//Cancel Merge
                {
                    var CancelToMerge = string.Join(",", selectedTables);
                    var success = await CancelMergedTablesAsync(CancelToMerge);
                    isok = success;
                }
                else if (MoveTable)
                {
                    var Moved = selectedTables.FirstOrDefault();
                    var success = await MoveTablesAsync(Moved);
                    isok = success;
                }
                if (isok)
                {
                    await Shell.Current.DisplayAlert("Success", "تم دمج الطاولات بنجاح", "OK");
                    try
                    {
                        await Shell.Current.GoToAsync($"//MapPage/PosPage");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"خطأ في التنقل: {ex.Message}");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "حدث خطأ أثناء دمج الطاولات", "OK");
                }
            }
        }

        private async Task<bool> SaveMergedTablesAsync(string tablesToMerge)
        {
            try
            {
                TbService tbService = new TbService
                {
                    SeMargeTo = tablesToMerge,
                    SeMenuId = MenuId
                };
                return await serviceServices.UpdateMerageTable(tbService);
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CancelMergedTablesAsync(string tablesToMerge)
        {
            try
            {
                TbService tbService = new TbService
                {
                    SeMargeTo = tablesToMerge,
                    SeMenuId = MenuId
                };
                return await serviceServices.CancelMerageTable(tbService);
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> MoveTablesAsync(int movedtables)
        {
            try
            {
                TbService tbService = new TbService
                {
                    SeTNumber = movedtables,
                    SeMenuId = MenuId
                };
                return await serviceServices.MoveTable(tbService);
            }
            catch
            {
                return false;
            }
        }
    }

}

