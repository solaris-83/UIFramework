using System.ComponentModel;

namespace UIFramework
{
    public enum ElementStatusEnum
    {
        [Description("info")]
        Info = 0,
        [Description("success")]
        Success,
        [Description("error")]
        Error,
        [Description("danger")]
        Danger,
        [Description("warning")]
        Warning,
        [Description("primary")]
        Primary,
        [Description("normal")]
        Normal,
        [Description("completed")]
        Completed,
        [Description("active")]
        Active,
        [Description("valid")]
        Valid,
        [Description("result")]
        Result,
        [Description("link-like")]
        LinkLike,
        [Description("led")]
        Led,
        [Description("card")]
        Card,
        [Description("radiobutton")]
        RadioButton,
        [Description("checkbox")]
        Checkbox,
        [Description("header")]
        Header,
        [Description("mandatory")]
        Mandatory,
        [Description("injector")]
        Injector
    }
}
