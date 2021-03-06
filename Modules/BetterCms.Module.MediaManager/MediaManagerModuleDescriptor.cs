﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaManagerModuleDescriptor.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Accessors;
using BetterCms.Module.MediaManager.Provider;
using BetterCms.Module.MediaManager.Registration;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Accessors;
using BetterCms.Module.Root.Providers;

using BetterModules.Core.Dependencies;
using BetterModules.Core.Modules.Registration;
using BetterModules.Events;

using Common.Logging;

namespace BetterCms.Module.MediaManager
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class MediaManagerModuleDescriptor : CmsModuleDescriptor
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "media";

        /// <summary>
        /// The media manager area name
        /// </summary>
        internal const string MediaManagerAreaName = "bcms-media";
        
        /// <summary>
        /// The media manager schema name
        /// </summary>
        internal const string MediaManagerSchemaName = "bcms_media";

        /// <summary>
        /// DateTime format for hard loading media images
        /// </summary>
        internal const string HardLoadImageDateTimeFormat = "yyMMddHHmmss";

        /// <summary>
        /// The media java script module descriptor.
        /// </summary>
        private readonly MediaManagerJsModuleIncludeDescriptor mediaJsModuleIncludeDescriptor;

        /// <summary>
        /// The media history java script module descriptor.
        /// </summary>
        private readonly MediaHistoryJsModuleIncludeDescriptor mediaHistoryJsModuleIncludeDescriptor;

        /// <summary>
        /// The media upload module descriptor.
        /// </summary>
        private readonly MediaUploadJsModuleIncludeDescriptor mediaUploadModuleIncludeDescriptor;

        /// <summary>
        /// The image editor module descriptor.
        /// </summary>
        private readonly ImageEditorJsModuleIncludeDescriptor imageEditorModuleIncludeDescriptor;

        /// <summary>
        /// The file editor module include descriptor.
        /// </summary>
        private readonly FileEditorJsModuleIncludeDescriptor fileEditorModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerModuleDescriptor" /> class.
        /// </summary>
        public MediaManagerModuleDescriptor(ICmsConfiguration cmsConfiguration) : base(cmsConfiguration)
        {
            mediaJsModuleIncludeDescriptor = new MediaManagerJsModuleIncludeDescriptor(this);
            mediaUploadModuleIncludeDescriptor = new MediaUploadJsModuleIncludeDescriptor(this);
            imageEditorModuleIncludeDescriptor = new ImageEditorJsModuleIncludeDescriptor(this);
            fileEditorModuleIncludeDescriptor = new FileEditorJsModuleIncludeDescriptor(this);
            mediaHistoryJsModuleIncludeDescriptor = new MediaHistoryJsModuleIncludeDescriptor(this);
            CategoryAccessors.Register<MediaFileCategoryAccessor>();
            CategoryAccessors.Register<MediaImageCategoryAccessor>();

            // Register images gallery custom option: album
            CustomOptionsProvider.RegisterProvider(MediaManagerFolderOptionProvider.Identifier, new MediaManagerFolderOptionProvider());
            CustomOptionsProvider.RegisterProvider(MediaManagerImageUrlOptionProvider.Identifier, new MediaManagerImageUrlOptionProvider());

            Events.MediaManagerEvents.Instance.MediaFileDeleted += Instance_MediaFileDeleted;
        }

        private void Instance_MediaFileDeleted(SingleItemEventArgs<MediaFile> args)
        {
            try
            {
                var lifetimeScope = ContextScopeProvider.CreateChildContainer();
                if (!lifetimeScope.IsRegistered<ICmsConfiguration>())
                {
                    throw new CmsApiException(string.Format("A '{0}' is unknown type in the Better CMS scope.", typeof(ICmsConfiguration).FullName));
                }
                var cmsConfiguration = lifetimeScope.Resolve<ICmsConfiguration>();
                if (cmsConfiguration.Storage.MoveDeletedFilesToTrash)
                {
                    if (!lifetimeScope.IsRegistered<IMediaFileService>())
                    {
                        throw new CmsApiException(string.Format("A '{0}' is unknown type in the Better CMS scope.", typeof(IMediaFileService).FullName));
                    }
                    var mediaFileService = lifetimeScope.Resolve<IMediaFileService>();
                    mediaFileService.MoveFilesToTrashFolder();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to start up deleted media trash collector.", ex);
            }
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of pages module.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "A media manager module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName
        {
            get
            {
                return MediaManagerAreaName;
            }
        }

        /// <summary>
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public override string SchemaName
        {
            get
            {
                return MediaManagerSchemaName;
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public override int Order
        {
            get
            {
                return int.MaxValue - 200;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultMediaFileUrlResolver>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultMediaFileService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultMediaImageService>().AsImplementedInterfaces().InstancePerLifetimeScope();            
            containerBuilder.RegisterType<DefaultMediaService>().AsImplementedInterfaces().InstancePerLifetimeScope();            
            containerBuilder.RegisterType<DefaultMediaHistoryService>().AsImplementedInterfaces().InstancePerLifetimeScope();            
            containerBuilder.RegisterType<DefaultTagService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultMediaImageVersionPathService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>        
        /// <returns>Enumerator of known module style sheet files.</returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                {
                    new CssIncludeDescriptor(this, "bcms.media.css")
                };
        }

        /// <summary>
        /// Gets known client side modules in page module.
        /// </summary>        
        /// <returns>List of known client side modules in page module.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {            
            return new[]
                {
                    mediaJsModuleIncludeDescriptor,
                    mediaUploadModuleIncludeDescriptor,
                    imageEditorModuleIncludeDescriptor,
                    fileEditorModuleIncludeDescriptor,
                    mediaHistoryJsModuleIncludeDescriptor,
                    new JsIncludeDescriptor(this, "bcms.html5Upload"),
                    new JsIncludeDescriptor(this, "bcms.jquery.jcrop"),
                    new JsIncludeDescriptor(this, "bcms.contextMenu")
                };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>List of page action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new LinkActionProjection(mediaJsModuleIncludeDescriptor, page => "loadSiteSettingsMediaManager")
                        {
                            Order = 2400,
                            Title = page => MediaGlobalization.SiteSettings_MediaManagerMenuItem,
                            CssClass = page => "bcms-settings-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent)
                        }                                      
                };
        }
    }
}
