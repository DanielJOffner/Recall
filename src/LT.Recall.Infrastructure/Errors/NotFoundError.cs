using LT.Recall.Domain.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.Properties;

namespace LT.Recall.Infrastructure.Errors
{
    public class NotFoundError : BaseError
    {
        private object _identifier;
        private Type _type;
        public NotFoundError(object identifier, Type type)
        {
            this._identifier = identifier;
            this._type = type;
        }

        protected override int ErrorCode => (int)InfraErrorCode.NotFound;
        protected override Enum Error => InfraErrorCode.NotFound;

        public override string GetUserFriendlyError()
        {
            return string.Format(Resources.ItemNotFoundError, _type.Name, _identifier);
        }
    }
}
