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

        public override void Act()
        {
            Texture2D image = mc._image.GetCurrentImage();

            if (image == null)
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка при сохранении файла",
                        "Нет изображения для сохранения",
                        null));
                return;
            }

            if (FileBrowserSolver.GetTypeOfPath(fileSave.currDir) == "Диск")
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка при сохранении файла",
                        "Сохранение в корневую папку диска не допускается",
                        null));
                return;
            }

            string fileName = fileSave.InputFileName.stringInput;

            if (!string.IsNullOrEmpty(fileName) && char.IsDigit(fileName[0]))
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка при сохранении файла",
                        "Имя файла не может начинаться с цифры",
                        null));
                return;
            }

            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in fileName)
            {
                if (Array.IndexOf(invalidChars, c) >= 0)
                {
                    string invalidList = string.Join(" ", invalidChars);
                    mc._entityManager.AddEntity(
                        new DialogMessage(
                            mc,
                            "Ошибка при сохранении файла",
                            $"Имя файла содержит недопустимые символы: {invalidList}",
                            null));
                    return;
                }
            }

            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName).ToUpperInvariant();
            string[] reservedNames = { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
            if (Array.IndexOf(reservedNames, nameWithoutExt) >= 0)
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка при сохранении файла",
                        $"Имя \"{nameWithoutExt}\" является зарезервированным в Windows и не может быть использовано",
                        null));
                return;
            }

            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка при сохранении файла",
                        "Файл должен иметь расширение .png или .jpg",
                        null));
                return;
            }

            string fullPath = Path.Combine(fileSave.currDir, fileName);
            if (File.Exists(fullPath))
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка при сохранении файла",
                        "Файл с таким названием уже есть в папке. Перезапись не допускается",
                        null));
                return;
            }

            try
            {
                Directory.CreateDirectory(fileSave.currDir);

                using (var stream = File.Create(fullPath))
                {
                    image.SaveAsPng(stream, image.Width, image.Height);
                }
            }
            catch (Exception ex)
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка при сохранении файла",
                        FileBrowserSolver.TrimExceptionMessage(ex),
                        null));
            }
        }

        public ActionSaveImage(Maincode imc, DialogFileSave fs) : base(imc)
        {
            fileSave = fs;
        }
    }
}
