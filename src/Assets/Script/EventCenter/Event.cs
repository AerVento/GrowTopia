using System;

namespace GrowTopia.Events
{
    /// <summary>
    /// This is a delegate class which supports add, remove and trigger, except clear for safety reason.
    /// </summary>
    public class Event<T> where T : Delegate
    {
        private T @event = null;
        public Event()
        {

        }
        public Event(T @event)
        {
            this.@event = @event;
        }

        /// <summary>
        /// Add delegate to current delegate.
        /// </summary>
        /// <param name="other"></param>
        public void Add(T other)
        {
            @event = (T)Delegate.Combine(@event, other);
        }

        /// <summary>
        /// Remove delegate from delegate.
        /// </summary>
        /// <param name="other"></param>
        public void Remove(T other)
        {
            Delegate.Remove(@event, other);
        }

        /// <summary>
        /// Trigger the delegate with arguments.
        /// NOTICE that the arguments can be any type.
        /// </summary>
        /// <param name="args"></param>
        public void Trigger(params object[] args)
        {
            try
            {
                @event?.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public static Event<T> operator +(Event<T> left, Event<T> right)
        {
            left.Add(right.@event);
            return left;
        }

        public static Event<T> operator +(Event<T> left, T right)
        {
            left.Add(right);
            return left;
        }

        public static Event<T> operator -(Event<T> left, Event<T> right)
        {
            left.Remove(right.@event);
            return left;
        }

        public static Event<T> operator -(Event<T> left, T right)
        {
            left.Remove(right);
            return left;
        }
    }
}