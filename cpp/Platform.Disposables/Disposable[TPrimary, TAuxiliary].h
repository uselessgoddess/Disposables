﻿namespace Platform::Disposables
{
    template <typename ...> class Disposable;
    template <typename TPrimary, typename TAuxiliary> class Disposable<TPrimary, TAuxiliary> : public Disposable<TPrimary>
    {
        public: const TAuxiliary AuxiliaryObject;

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject, std::function<void(TPrimary, TAuxiliary)> action)
            : Disposable<TPrimary>(object)
        {
            AuxiliaryObject = auxiliaryObject;
            OnDispose += [&](auto manual, auto wasDisposed)
            {
                if (!wasDisposed)
                {
                    this->action(Object, AuxiliaryObject);
                }
            };
        }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject, std::function<void()> action) : Disposable<TPrimary>(object, action) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject, Disposal disposal) : Disposable<TPrimary>(object, disposal) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject) : Disposable<TPrimary>(object) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object) : Disposable<TPrimary>(object) { }

        public: Disposable(std::tuple<TPrimary, TAuxiliary, std::function<void(TPrimary, TAuxiliary)>> tuple) : Disposable(std::get<0>(tuple), std::get<1>(tuple), std::get<2>(tuple)) { }

        public: Disposable(std::tuple<TPrimary, TAuxiliary, std::function<void()>> tuple) : Disposable(std::get<0>(tuple), std::get<1>(tuple), std::get<2>(tuple)) { }

        public: Disposable(std::tuple<TPrimary, TAuxiliary, Disposal> tuple) : Disposable(std::get<0>(tuple), std::get<1>(tuple), std::get<2>(tuple)) { }

        public: Disposable(std::tuple<TPrimary, TAuxiliary> tuple) : Disposable(std::get<0>(tuple), std::get<1>(tuple)) { }

        public: operator TPrimary() const { return this->Object; }

        public: operator TAuxiliary() const { return this->AuxiliaryObject; }

        protected: void Dispose(bool manual, bool wasDisposed) override
        {
            this->RaiseOnDisposeEvent(manual, wasDisposed);
            AuxiliaryObject.TryDispose();
            Object.TryDispose();
        }
    };
}
