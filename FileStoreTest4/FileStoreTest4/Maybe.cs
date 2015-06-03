using System;
using System.Collections;
using System.Collections.Generic;

namespace FileStoreTest4
{
    public class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> values;

        public Maybe()
        {
            this.values = new T[0];
        }

        public Maybe(T value)
        {
            // Guard clause to stop a null being inputted
            if (value == null)
                throw new ArgumentNullException("Cannot be null - use return new Maybe<string>()");

            this.values = new[] { value };
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
