﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageModel.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    [DataContract]
    [Serializable]
    public class PageModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether page exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page exists; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Exists { get; set; }

        /// <summary>
        /// Gets or sets the existing page id .
        /// </summary>
        /// <value>
        /// The existing page id.
        /// </value>
        [DataMember]
        public Guid? PageId { get; set; }
    }
}