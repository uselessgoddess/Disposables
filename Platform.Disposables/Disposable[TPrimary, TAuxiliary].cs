﻿using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Represents disposable container that disposes two contained objects when the container itself is disposed.</para>
    /// <para>Представляет высвобождаемый контейнер, который высвобождает два содержащийхся в нём объектов при высвобождении самого контейнера.</para>
    /// </summary>
    public class Disposable<TPrimary, TAuxiliary> : Disposable<TPrimary>
    {
        public TAuxiliary AuxiliaryObject { get; }

        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject, Action<TPrimary, TAuxiliary> action)
            : base(@object)
        {
            AuxiliaryObject = auxiliaryObject;
            OnDispose += (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    action(Object, AuxiliaryObject);
                }
            };
        }

        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject, Action action) : base(@object, action) => AuxiliaryObject = auxiliaryObject;

        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject, Disposal disposal) : base(@object, disposal) => AuxiliaryObject = auxiliaryObject;

        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject) : base(@object) => AuxiliaryObject = auxiliaryObject;

        public Disposable(TPrimary @object) : base(@object) { }

        public static implicit operator Disposable<TPrimary, TAuxiliary>(ValueTuple<TPrimary, TAuxiliary, Action<TPrimary, TAuxiliary>> tuple) => new Disposable<TPrimary, TAuxiliary>(tuple.Item1, tuple.Item2, tuple.Item3);

        public static implicit operator Disposable<TPrimary, TAuxiliary>(ValueTuple<TPrimary, TAuxiliary, Action> tuple) => new Disposable<TPrimary, TAuxiliary>(tuple.Item1, tuple.Item2, tuple.Item3);

        public static implicit operator Disposable<TPrimary, TAuxiliary>(ValueTuple<TPrimary, TAuxiliary, Disposal> tuple) => new Disposable<TPrimary, TAuxiliary>(tuple.Item1, tuple.Item2, tuple.Item3);

        public static implicit operator Disposable<TPrimary, TAuxiliary>(ValueTuple<TPrimary, TAuxiliary> tuple) => new Disposable<TPrimary, TAuxiliary>(tuple.Item1, tuple.Item2);

        public static implicit operator TPrimary(Disposable<TPrimary, TAuxiliary> disposableContainer) => disposableContainer.Object;

        public static implicit operator TAuxiliary(Disposable<TPrimary, TAuxiliary> disposableContainer) => disposableContainer.AuxiliaryObject;

        protected override void Dispose(bool manual, bool wasDisposed)
        {
            RaiseOnDisposeEvent(manual, wasDisposed);
            AuxiliaryObject.TryDispose();
            Object.TryDispose();
        }
    }
}