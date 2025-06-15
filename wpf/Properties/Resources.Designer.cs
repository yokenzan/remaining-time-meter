// <copyright file="Resources.Designer.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

namespace RemMeter.Properties
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources
    {
        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources()
        {
        }

        /// <summary>
        /// Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RemMeter.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }

                return resourceMan;
            }
        }

        /// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }

            set
            {
                resourceCulture = value;
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Remaining Time Meter.
        /// </summary>
        public static string AppTitle
        {
            get
            {
                return ResourceManager.GetString("AppTitle", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Display {0} - {1}x{2}.
        /// </summary>
        public static string DisplayFormat
        {
            get
            {
                return ResourceManager.GetString("DisplayFormat", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Display {0} (Primary) - {1}x{2}.
        /// </summary>
        public static string DisplayFormatPrimary
        {
            get
            {
                return ResourceManager.GetString("DisplayFormatPrimary", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Display Monitor: .
        /// </summary>
        public static string DisplayMonitor
        {
            get
            {
                return ResourceManager.GetString("DisplayMonitor", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Error.
        /// </summary>
        public static string Error
        {
            get
            {
                return ResourceManager.GetString("Error", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to An error occurred: {0}.
        /// </summary>
        public static string ErrorOccurred
        {
            get
            {
                return ResourceManager.GetString("ErrorOccurred", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid minutes value.
        /// </summary>
        public static string InvalidMinutesValue
        {
            get
            {
                return ResourceManager.GetString("InvalidMinutesValue", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid seconds value (0-59).
        /// </summary>
        public static string InvalidSecondsValue
        {
            get
            {
                return ResourceManager.GetString("InvalidSecondsValue", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to minutes.
        /// </summary>
        public static string Minutes
        {
            get
            {
                return ResourceManager.GetString("Minutes", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Pause.
        /// </summary>
        public static string Pause
        {
            get
            {
                return ResourceManager.GetString("Pause", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Please set the time correctly.
        /// </summary>
        public static string PleaseSetTimeCorrectly
        {
            get
            {
                return ResourceManager.GetString("PleaseSetTimeCorrectly", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Position: .
        /// </summary>
        public static string Position
        {
            get
            {
                return ResourceManager.GetString("Position", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Bottom edge.
        /// </summary>
        public static string PositionBottom
        {
            get
            {
                return ResourceManager.GetString("PositionBottom", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Left edge.
        /// </summary>
        public static string PositionLeft
        {
            get
            {
                return ResourceManager.GetString("PositionLeft", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Right edge.
        /// </summary>
        public static string PositionRight
        {
            get
            {
                return ResourceManager.GetString("PositionRight", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Top edge.
        /// </summary>
        public static string PositionTop
        {
            get
            {
                return ResourceManager.GetString("PositionTop", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Resume.
        /// </summary>
        public static string Resume
        {
            get
            {
                return ResourceManager.GetString("Resume", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to seconds.
        /// </summary>
        public static string Seconds
        {
            get
            {
                return ResourceManager.GetString("Seconds", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Start.
        /// </summary>
        public static string Start
        {
            get
            {
                return ResourceManager.GetString("Start", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Stop.
        /// </summary>
        public static string Stop
        {
            get
            {
                return ResourceManager.GetString("Stop", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Timer.
        /// </summary>
        public static string Timer
        {
            get
            {
                return ResourceManager.GetString("Timer", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Time Setting: .
        /// </summary>
        public static string TimeSetting
        {
            get
            {
                return ResourceManager.GetString("TimeSetting", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Time's up! {0} minutes {1} seconds have passed!.
        /// </summary>
        public static string TimeUpMessage
        {
            get
            {
                return ResourceManager.GetString("TimeUpMessage", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to Remember my settings.
        /// </summary>
        public static string RememberMySettings
        {
            get
            {
                return ResourceManager.GetString("RememberMySettings", resourceCulture);
            }
        }
    }
}