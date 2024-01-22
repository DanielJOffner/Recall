﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LT.Recall.Cli.Properties {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LT.Recall.Cli.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Storage collection..
        /// </summary>
        public static string CollectionOptionHelpText {
            get {
                return ResourceManager.GetString("CollectionOptionHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleted {0}..
        /// </summary>
        public static string CommandsDeletedMessage {
            get {
                return ResourceManager.GetString("CommandsDeletedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Started in test mode. Press ENTER to continue (process id is {0})..
        /// </summary>
        public static string DebugMessage {
            get {
                return ResourceManager.GetString("DebugMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancelled..
        /// </summary>
        public static string DeleteAbortedMessage {
            get {
                return ResourceManager.GetString("DeleteAbortedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No commands found..
        /// </summary>
        public static string DeleteNoCommandsFoundMessage {
            get {
                return ResourceManager.GetString("DeleteNoCommandsFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to usage:
        ///    delete -c &quot;infrastructure&quot; -t &quot;kubernetes,aws&quot;.
        /// </summary>
        public static string DeleteVerbExampleText {
            get {
                return ResourceManager.GetString("DeleteVerbExampleText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete commands..
        /// </summary>
        public static string DeleteVerbHelpText {
            get {
                return ResourceManager.GetString("DeleteVerbHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command Text: .
        /// </summary>
        public static string EnterCommandTextMessage {
            get {
                return ResourceManager.GetString("EnterCommandTextMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Description: .
        /// </summary>
        public static string EnterDescriptionMessage {
            get {
                return ResourceManager.GetString("EnterDescriptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tags (Comma Separated): .
        /// </summary>
        public static string EnterTagsMessage {
            get {
                return ResourceManager.GetString("EnterTagsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to usage: 
        ///    export -p C:\Files\Export\commands.csv.
        /// </summary>
        public static string ExportVerbExampleText {
            get {
                return ResourceManager.GetString("ExportVerbExampleText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Export commands..
        /// </summary>
        public static string ExportVerbHelpText {
            get {
                return ResourceManager.GetString("ExportVerbHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Show help..
        /// </summary>
        public static string HelpOptionHelpText {
            get {
                return ResourceManager.GetString("HelpOptionHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Help text not configured..
        /// </summary>
        public static string HelpTextNotFoundError {
            get {
                return ResourceManager.GetString("HelpTextNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to usage:
        ///    import -p C:\Files\Import\commands.csv.
        /// </summary>
        public static string ImportVerbExampleText {
            get {
                return ResourceManager.GetString("ImportVerbExampleText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Import commands..
        /// </summary>
        public static string ImportVerbHelpText {
            get {
                return ResourceManager.GetString("ImportVerbHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to usage:
        ///    install linux
        ///
        ///To see all available collections, please visit https://github.com/DanielJOffner/Recall/. .
        /// </summary>
        public static string InstallVerbExampleText {
            get {
                return ResourceManager.GetString("InstallVerbExampleText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Install collections..
        /// </summary>
        public static string InstallVerbHelpText {
            get {
                return ResourceManager.GetString("InstallVerbHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid arguments. Use &apos;{0} --help&apos; for details..
        /// </summary>
        public static string InvalidArgumentsError {
            get {
                return ResourceManager.GetString("InvalidArgumentsError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to List installers and collections..
        /// </summary>
        public static string ListAllOptionHelpText {
            get {
                return ResourceManager.GetString("ListAllOptionHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Make selection and press ENTER or CTRL+C to exit..
        /// </summary>
        public static string MakeSelectionMessage {
            get {
                return ResourceManager.GetString("MakeSelectionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Absolute path..
        /// </summary>
        public static string PathOptionHelpText {
            get {
                return ResourceManager.GetString("PathOptionHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Are you sure you want to delete {0} command(s)? [Y/N]:.
        /// </summary>
        public static string PreviewDeleteMessage {
            get {
                return ResourceManager.GetString("PreviewDeleteMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save a new command..
        /// </summary>
        public static string SaveCommandHelpText {
            get {
                return ResourceManager.GetString("SaveCommandHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Saved command: {0}.
        /// </summary>
        public static string SaveSuccessMessage {
            get {
                return ResourceManager.GetString("SaveSuccessMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to usage: 
        ///    save -c &quot;infrastructure&quot;.
        /// </summary>
        public static string SaveVerbExampleText {
            get {
                return ResourceManager.GetString("SaveVerbExampleText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save a new command..
        /// </summary>
        public static string SaveVerbHelpText {
            get {
                return ResourceManager.GetString("SaveVerbHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to usage:
        ///    search linux chmod.
        /// </summary>
        public static string SearchVerbExampleText {
            get {
                return ResourceManager.GetString("SearchVerbExampleText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search commands..
        /// </summary>
        public static string SearchVerbHelpText {
            get {
                return ResourceManager.GetString("SearchVerbHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Storage statistics..
        /// </summary>
        public static string StatsVerbHelpText {
            get {
                return ResourceManager.GetString("StatsVerbHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Comma separated list of tags..
        /// </summary>
        public static string TagsOptionHelpText {
            get {
                return ResourceManager.GetString("TagsOptionHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown error occurred..
        /// </summary>
        public static string UnknownError {
            get {
                return ResourceManager.GetString("UnknownError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enable verbose output..
        /// </summary>
        public static string VerboseOptionHelpText {
            get {
                return ResourceManager.GetString("VerboseOptionHelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start in test mode..
        /// </summary>
        public static string XTestOptionHelpText {
            get {
                return ResourceManager.GetString("XTestOptionHelpText", resourceCulture);
            }
        }
    }
}
