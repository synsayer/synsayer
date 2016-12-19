namespace FrameWork.View
{
    public abstract class ViewBase : FrameWork.Behaviour.CommonBehaviour
    {
        public ViewBase()
        {
            Controller.ControllerBase.init();
            Controller.ControllerBase.addView(this);  //controller에 View 등록
        }
        virtual protected void Awake()
        {
            init();

        }

        virtual protected void OnDestroy()
        {
            if (enabled)
            {
                destroy();
                RemoveAllEventListener();
            }
        }

        public virtual void init()
        {
            registerEventListener();
        }

        public virtual void destroy()
        {
            unregisterEventListener();
            Controller.ControllerBase.removeView(this); //controller에 View 등록해제
        }

        protected virtual void registerEventListener()
        {

        }

        protected virtual void unregisterEventListener()
        {

        }

    }
}