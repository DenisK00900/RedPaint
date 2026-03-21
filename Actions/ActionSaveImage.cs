using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedPaint
{
    public class ActionSaveImage : AbstrAction
    {
        DialogFileSave fileSave;

        bool forse = false;
        bool ignoneLayers = false;
        public override void Act()
        {
            Texture2D image = TUH.GetCombineBeforeSave(mc);

            string fileName = fileSave.InputFileName.stringInput;
            string fullPath = Path.Combine(fileSave.currDir, fileName);

            if (!forse)
            {
                if (image == null)
                {
                    mc._entityManager.AddEntity(
                        new DialogError(
                            mc,
                            "Ошибка при сохранении файла",
                            "Нет изображения для сохранения",
                            null));
                    succCall = false;
                    return;
                }

                if (FileBrowserSolver.GetTypeOfPath(fileSave.currDir) == "Диск")
                {
                    mc._entityManager.AddEntity(
                        new DialogError(
                            mc,
                            "Ошибка при сохранении файла",
                            "Сохранение в корневую папку диска не допускается",
                            null));
                    succCall = false;
                    return;
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    mc._entityManager.AddEntity(
                        new DialogError(
                            mc,
                            "Ошибка при сохранении файла",
                            "Пустое имя файла. Введите название и расширение файла.",
                            null));
                    succCall = false;
                    return;
                }

                char[] invalidChars = Path.GetInvalidFileNameChars();
                foreach (char c in fileName)
                {
                    if (Array.IndexOf(invalidChars, c) >= 0)
                    {
                        string invalidList = string.Join(" ", invalidChars);
                        mc._entityManager.AddEntity(
                            new DialogError(
                                mc,
                                "Ошибка при сохранении файла",
                                $"Имя файла содержит недопустимые символы: {invalidList}",
                                null));
                        succCall = false;
                        return;
                    }
                }

                string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName).ToUpperInvariant();
                string[] reservedNames = { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
                if (Array.IndexOf(reservedNames, nameWithoutExt) >= 0)
                {
                    mc._entityManager.AddEntity(
                        new DialogError(
                            mc,
                            "Ошибка при сохранении файла",
                            $"Имя \"{nameWithoutExt}\" является зарезервированным в Windows и не может быть использовано",
                            null));
                    succCall = false;
                    return;
                }

                string ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                {
                    mc._entityManager.AddEntity(
                        new DialogError(
                            mc,
                            "Ошибка при сохранении файла",
                            "Файл должен иметь расширение .png или .jpg",
                            null));
                    succCall = false;
                    return;
                }

                if (!ignoneLayers)
                {
                    if (mc._image.layers.Count > 1)
                    {
                        DialogWarning dw = new DialogWarning(
                            mc,
                            "Ошибка при сохранении файла",
                            "Данный тип файлов не поддерживать множественные слои.\nПри сохранении слои будут объеденены. Продолжить?",
                            null);

                        dw.SetAgreeText("Объединить");

                        ActionSaveImage forseSave = new ActionSaveImage(mc, fileSave);
                        forseSave.ignoneLayers = true;

                        dw.agree.AddAction(forseSave);
                        dw.agree.AddAction(new ActionDestroy(mc, dw));
                        dw.disagree.AddAction(new ActionDestroy(mc, fileSave));

                        dw.agree.hint = new Hint(mc, "Объединить слои и сохранить в этом формате");

                        dw.SetDisagreeText("Отмена");

                        dw.disagree.AddAction(new ActionDestroy(mc, dw));
                        dw.disagree.AddAction(new ActionDestroy(mc, fileSave));

                        dw.disagree.hint = new Hint(mc, "Отменить действие и не объединять слои");

                        mc._entityManager.AddEntity(dw);

                        succCall = false;
                        return;
                    }
                }

                if (File.Exists(fullPath))
                {
                    DialogWarning dw = new DialogWarning(
                            mc,
                            "Ошибка при сохранении файла",
                            "Файл с таким названием уже есть в папке. Хотите перезаписать файл?",
                            null);

                    dw.SetAgreeText("Перезаписать");

                    ActionSaveImage forseSave = new ActionSaveImage(mc, fileSave);
                    forseSave.forse = true;

                    dw.agree.AddAction(forseSave);
                    dw.agree.AddAction(new ActionDestroy(mc, dw));
                    dw.disagree.AddAction(new ActionDestroy(mc, fileSave));

                    dw.agree.hint = new Hint(mc, "Перезаписать файл " + fileName + ".\nСодержимое текущего файла будет уничтожено");

                    dw.SetDisagreeText("Отмена");

                    dw.disagree.AddAction(new ActionDestroy(mc, dw));
                    dw.disagree.AddAction(new ActionDestroy(mc, fileSave));

                    dw.disagree.hint = new Hint(mc, "Отменить действие и не перезаписывать файл.\nВы можете сохранить его под другим именем");

                    mc._entityManager.AddEntity(dw);

                    succCall = false;
                    return;
                }
            }

            try
            {
                Directory.CreateDirectory(fileSave.currDir);

                using (var stream = File.Create(fullPath))
                {
                    image.SaveAsPng(stream, image.Width, image.Height);
                }

                mc._image.isModified = false;
            }
            catch (Exception ex)
            {
                mc._entityManager.AddEntity(
                    new DialogError(
                        mc,
                        "Ошибка при сохранении файла",
                        FileBrowserSolver.TrimExceptionMessage(ex),
                        null));
                succCall = false;
            }
        }

        public ActionSaveImage(Maincode imc, DialogFileSave fs) : base(imc)
        {
            fileSave = fs;
        }
    }
}
