
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentWorker.Models
{
    public static class SelectFolderOrFile
    {
        public static void AddExcelOrdersFiles(ObservableCollection<string>? list)
        {
            var dialog = openExcelFileDialog(true, "Excel files (*.xlsx)|*.xlsx");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in dialog.FileNames)
                {
                    list.Add(Path.GetFullPath(filename));
                }
            }
        }

        public static string AddExcelPriceFile()
        {
            var dialog = openExcelFileDialog(false, "Excel files (*.xlsx)|*.xlsx");
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var file = Path.GetFullPath(dialog.FileName);
                return file;
            }

            return "Прайс-лист не выбран";
        }

        public static string SelectFolderToSave()
        {
            FolderBrowserDialog openFileDlg = new();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                var folderPath = openFileDlg.SelectedPath;
                return folderPath;
            }
            return "Вы не выбрали папку для сохранения";
        }

        /// <summary>
        /// Открывает файловый диалог.
        /// Пример фильтра "Excel files (*.xlsx)|*.xlsx"
        /// </summary>
        /// <param name="multiselect"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static OpenFileDialog openExcelFileDialog(bool multiselect, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = multiselect;
            openFileDialog.Filter = filter;
            return openFileDialog;
        }
    }
}
