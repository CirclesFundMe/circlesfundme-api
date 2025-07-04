namespace CirclesFundMe.Application.Helpers
{
    public class UtilityHelper(ILogger<UtilityHelper> logger)
    {
        private readonly ILogger<UtilityHelper> _logger = logger;

        public async Task ExecuteWithRetryAsync(Func<Task> action)
        {
            AsyncRetryPolicy policy = Policy.Handle<Exception>()
                                           .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                               (exception, timeSpan, retryCount, context) =>
                                               {
                                                   _logger.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Waiting {timeSpan} before next retry.");
                                               });

            await policy.ExecuteAsync(action);
        }

        public static Dictionary<string, string> FormatDecimalProperties(object obj)
        {
            return obj.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?))
                .ToDictionary(
                    p => p.Name,
                    p =>
                    {
                        object? value = p.GetValue(obj);
                        decimal decimalValue = value == null ? 0 : Convert.ToDecimal(value);
                        return Math.Round(decimalValue).ToString("N0", CultureInfo.InvariantCulture);
                    }
                );
        }

        public static string GenerateOtp()
        {
            byte[] randomNumber = new byte[4];
            RandomNumberGenerator.Fill(randomNumber);

            int randomInt = BitConverter.ToInt32(randomNumber, 0);

            randomInt = Math.Abs(randomInt);

            int otp = randomInt % 1000000;

            return otp.ToString("D6");
        }

        public static string GenerateDefaultPassword(int length = 12)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            StringBuilder password = new();
            Random random = new();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }

            return password.ToString();
        }

        public static string CapitalizeFirstLetters(params string[] words)
        {
            return string.Join(" ", words.Select(word => char.ToUpper(word.Trim()[0]) + word.Trim()[1..].ToLower()));
        }

        public static string NormalizeLower(string value)
        {
            return value.Trim().ToLower();
        }

        public static string Serializer(object obj)
        {
            JsonSerializerSettings settings = new()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            return JsonConvert.SerializeObject(obj, settings);
        }

        public T? Deserializer<T>(string json)
        {
            T? result = default;
            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Deserialization error occurred with - {json} :: {ex.Message}\n");
            }

            return result;
        }

        public static bool ShouldMapMember(object srcMember)
        {
            return srcMember != null && !IsDefaultValue(srcMember);
        }

        static bool IsDefaultValue(object srcMember)
        {
            if (srcMember is string str)
            {
                return string.IsNullOrEmpty(str);
            }

            Type type = srcMember.GetType();

            if (type.IsValueType)
            {
                object? defaultValue = Activator.CreateInstance(type);

                if (type == typeof(bool))
                {
                    return false;
                }

                if (defaultValue != null)
                {
                    return defaultValue.Equals(srcMember);
                }
            }

            return false;
        }
    }
}
