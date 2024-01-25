using System.Text.Json;

namespace XYZHotel.Domain.Conf
{
    public class PascalCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // Vérifiez si les noms sont déjà en PascalCase
            if (string.IsNullOrEmpty(name) || char.IsUpper(name[0]))
            {
                return name;
            }

            // Convertissez le premier caractère en majuscule
            char[] chars = name.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
    }
}
