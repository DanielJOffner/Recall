using Newtonsoft.Json.Serialization;
using System.Reflection;
// ReSharper disable SimplifyLinqExpressionUseMinByAndMaxBy

namespace LT.Recall.Infrastructure.Serialization
{
    // Taken from : https://github.com/JamesNK/Newtonsoft.Json/issues/1570
    public class MostSpecificConstructorContractResolver : DefaultContractResolver
    {
        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);

            if (!objectType.IsAbstract && !objectType.IsInterface
                && contract.DefaultCreator == null && contract.OverrideCreator == null)
            {
                var constructor = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .OrderBy(c => c.GetParameters().Length)
                    .LastOrDefault();

                if (constructor != null)
                {
                    contract.OverrideCreator = a => constructor.Invoke(a);
                    foreach (var parameter in CreateConstructorParameters(constructor, contract.Properties))
                    {
                        contract.CreatorParameters.AddProperty(parameter);
                    }
                }
            }

            return contract;
        }
    }
}
