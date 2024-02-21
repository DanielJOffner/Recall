﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LT.Recall.Infrastructure.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LT.Recall.Infrastructure.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP GET {0} returned {1}. {2}.
        /// </summary>
        internal static string BadInstallUrlError {
            get {
                return ResourceManager.GetString("BadInstallUrlError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command with id {0} appears twice in the collection..
        /// </summary>
        internal static string DuplicateCommandError {
            get {
                return ResourceManager.GetString("DuplicateCommandError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GitHub installer could not locate the collection {0}..
        /// </summary>
        internal static string GitHubCollectionNotFoundError {
            get {
                return ResourceManager.GetString("GitHubCollectionNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Csv file at {0} is not a valid format..
        /// </summary>
        internal static string InvalidCsvFormatError {
            get {
                return ResourceManager.GetString("InvalidCsvFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No {0} found matching identifier {1}.
        /// </summary>
        internal static string ItemNotFoundError {
            get {
                return ResourceManager.GetString("ItemNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sate file could not be found at {0}.
        /// </summary>
        internal static string StateFileNotFoundError {
            get {
                return ResourceManager.GetString("StateFileNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown error occurred: {0}..
        /// </summary>
        internal static string UnknownError {
            get {
                return ResourceManager.GetString("UnknownError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is not a supported file type..
        /// </summary>
        internal static string UnsupportedFileTypeError {
            get {
                return ResourceManager.GetString("UnsupportedFileTypeError", resourceCulture);
            }
        }
    }
}
