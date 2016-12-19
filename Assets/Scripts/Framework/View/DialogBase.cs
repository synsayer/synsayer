using FrameWork.Event;
namespace FrameWork.View
{
    public abstract class DialogBase : ViewBase
	{
        public static string EVENT_ID_DIALOG_CLOSE = "EVENT_ID_DIALOG_CLOSE";

        public bool gs_bSkipBackButtonAction { get; set; }

        public override void init()
        {
            gs_bSkipBackButtonAction = false;
            base.init();
        }
        protected void dispatchDialogCloseEvent()
        {
            DispatchEvent(EVENT_ID_DIALOG_CLOSE, BasicEventArgs.argNone);    
        }

        abstract public void onClickBackButton();
	}
}