using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace DocumentWorker.Models
{
    public static class SelectFolderOrFile
    {
        /// <summary>
        /// Открывает файловый диалог с возможностью множественного
        /// выбора xlsx файлов и заполняет список абсолютными путями 
        /// к файлам
        /// </summary>
        /// <param name="list">Список для заполнения данными</param>
        public static void WorkingWithTheFileDialog(ObservableCollection<string>? list)
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

        /// <summary>
        /// Открывает файловый диалог без возможности множественного
        /// выбора xlsx файлов.
        /// </summary>
        /// <returns>строка с абсолютным путем к выбранному файлу</returns>
        public static string WorkingWithTheFileDialog()
        {
            var dialog = openExcelFileDialog(false, "Excel files (*.xlsx)|*.xlsx");
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var file = Path.GetFullPath(dialog.FileName);
                return file;
            }

            return "Прайс-лист не выбран";
        }

        /// <summary>
        /// Открывает диалог для выбора папки.
        /// </summary>
        /// <returns>строка с абсолютным путем к выбранной папке</returns>
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
        /// <param name="multiselect">Возможность множественного выбора true или false</param>
        /// <param name="filter">Фильтр файлов, которые можно выбрать</param>
        /// <returns>OpenFileDialog</returns>
        private static OpenFileDialog openExcelFileDialog(bool multiselect, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = multiselect;
            openFileDialog.Filter = filter;
            return openFileDialog;
        }
    }
}
