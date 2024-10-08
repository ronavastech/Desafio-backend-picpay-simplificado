namespace PicPaySimplificado.Utils;

public static class CPFCNPJValidator
{
    public static bool IsCpf(string cpf)
    {
        return IsValid(cpf, 11, new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 }, new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });
    }

    public static bool IsCnpj(string cnpj)
    {
        return IsValid(cnpj, 14, new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 }, new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });
    }

    public static bool IsValidCpfCnpj(string cpfCnpj)
    {
        return cpfCnpj.Length == 11 ? IsCpf(cpfCnpj) : (cpfCnpj.Length == 14 ? IsCnpj(cpfCnpj) : false);
    }

    private static bool IsValid(string value, int expectedLength, int[] firstMultipliers, int[] secondMultipliers)
    {
        if (value.Length != expectedLength || !long.TryParse(value, out _))
            return false;

        int firstSum = 0, secondSum = 0;

        // Calcular os dois dígitos verificadores em uma única iteração
        for (int i = 0; i < expectedLength - 2; i++)
        {
            var digit = int.Parse(value[i].ToString());
            firstSum += digit * firstMultipliers[i];
            secondSum += digit * secondMultipliers[i];
        }

        // Primeiro dígito verificador
        var firstVerifier = GetVerifier(firstSum);

        // Adiciona o primeiro dígito para o cálculo do segundo verificador
        secondSum += firstVerifier * secondMultipliers[expectedLength - 2];

        // Segundo dígito verificador
        var secondVerifier = GetVerifier(secondSum);

        // Comparação final
        return value.EndsWith(firstVerifier.ToString() + secondVerifier.ToString());
    }

    private static int GetVerifier(int sum)
    {
        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}