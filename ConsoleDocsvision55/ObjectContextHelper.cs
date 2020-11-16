using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Mapping;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.Data.Metadata;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectModel;
using DocsVision.Platform.ObjectModel.Mapping;
using DocsVision.Platform.ObjectModel.Persistence;
using DocsVision.Platform.SystemCards.ObjectModel.Mapping;
using DocsVision.Platform.SystemCards.ObjectModel.Services;
using System;

namespace ConsoleDocsvision55
{
    internal class ObjectContextHelper : IServiceProvider
    {
        private readonly UserSession session;
        private ObjectContext localContext;

        public ObjectContextHelper(UserSession session)
        {
            this.session = session;
        }

        public ObjectContext LocalContext
        {
            get
            {
                if (localContext == null)
                    CreateLocalContext();

                return localContext;
            }
        }

        /// <summary>
        /// Получение Id объекта
        /// </summary>
        public Guid GetCardId(ObjectBase card)
        {
            return localContext.GetObjectRef(card).Id;
        }

        /// <summary>
        /// Cброс кэша ролевой модели для новой карточки
        /// </summary>
        public void ResetRolesCache(ObjectBase card)
        {
            ExtensionMethod extensionMethod = session.ExtensionManager.GetExtensionMethod("BackOfficeExtension", "ResetRolesCache");
            if (extensionMethod == null)
                return;

            extensionMethod.Parameters.AddNew("cardId", ParameterValueType.Guid, GetCardId(card));
            extensionMethod.Execute();
        }

        private void CreateLocalContext()
        {
            {
                // если не получается сделать через IServiceProvider, то есть второй способ создания контекста, см. внизу
                try
                {
                    localContext = new ObjectContext(this);

                    IObjectMapperFactoryRegistry mapperFactoryRegistry = localContext.GetService<IObjectMapperFactoryRegistry>();
                    mapperFactoryRegistry.RegisterFactory(typeof(SystemCardsMapperFactory));
                    mapperFactoryRegistry.RegisterFactory(typeof(BackOfficeMapperFactory));

                    IServiceFactoryRegistry serviceFactoryRegistry = localContext.GetService<IServiceFactoryRegistry>();
                    serviceFactoryRegistry.RegisterFactory(typeof(SystemCardsServiceFactory));
                    serviceFactoryRegistry.RegisterFactory(typeof(BackOfficeServiceFactory));

                    localContext.AddService(DocsVisionObjectFactory.CreatePersistentStore(session));

                    IMetadataProvider metadataProvider = DocsVisionObjectFactory.CreateMetadataProvider(new SessionProvider(session));
                    localContext.AddService(metadataProvider);
                    localContext.AddService(DocsVisionObjectFactory.CreateMetadataManager(metadataProvider, session));

                    localContext.GetService<IAccessCheckingService>().EditMode = true;
                }
                catch
                {
                    if (localContext != null)
                    {
                        localContext.Dispose();
                        localContext = null;
                    }

                    throw;
                }
            }
        }

        #region IServiceProvider implementation

        object IServiceProvider.GetService(Type serviceType)
        {
            return serviceType == typeof(UserSession) ? session : null;
        }

        #endregion


    }
}
