using System;
using System.Text.RegularExpressions;

namespace IngameConsole {

    public class ConsoleFilter {
        private Predicate<ConsoleData> customFilter = null;

        private int logLevel;
        private bool useRegexForFilter = false;
        private Regex filterStringRegex;
        private string filterString = "";

        public Action<ConsoleFilter> OnValueChanged { get; set; }

        /// <summary>
        /// nullable
        /// </summary>
        public Predicate<ConsoleData> CustomFilter {
            get => customFilter;
            set {
                customFilter = value;
                OnValueChanged?.Invoke(this);
            }
        }

        public int LogLevel {
            get => logLevel;
            set {
                logLevel = value;
                OnValueChanged?.Invoke(this);
            }
        }

        public bool UseRegexForFilter {
            get => this.useRegexForFilter;
            set {
                this.useRegexForFilter = value;

                try {
                    if (this.useRegexForFilter) {
                        filterStringRegex = new Regex(filterString);
                    }
                } catch (ArgumentException) {
                    //Do Nothing
                } catch (Exception e) {
                    throw e;
                }

                OnValueChanged?.Invoke(this);
            }
        }

        public string FilterString {
            get => filterString;
            set {
                filterString = value;

                try {
                    if (this.useRegexForFilter) {
                        filterStringRegex = new Regex(filterString);
                    }
                } catch (ArgumentException) {
                    //Do Nothing
                } catch (Exception e) {
                    throw e;
                }

                OnValueChanged?.Invoke(this);
            }
        }

        public bool Check(ConsoleData data) {
            return LogLevelCheck(data.LogLevel) &&
                    StringFilterCheck(data.Msg) &&
                    (customFilter == null || customFilter.Invoke(data));
        }

        private bool LogLevelCheck(int logLevel) {
            return this.logLevel <= logLevel;
        }

        private bool StringFilterCheck(string str) {
            if (useRegexForFilter) {
                if (filterStringRegex == null || filterStringRegex.IsMatch(str)) {
                    return true;
                } else {
                    return false;
                }
            } else {
                if (0 <= str.IndexOf(filterString, StringComparison.OrdinalIgnoreCase)) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
}