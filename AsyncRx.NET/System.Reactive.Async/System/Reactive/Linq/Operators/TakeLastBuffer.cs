﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<IList<TSource>> TakeLastBuffer<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return Empty<IList<TSource>>();
            }

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.TakeLastBuffer(observer, count)));
        }

        public static IAsyncObservable<IList<TSource>> TakeLastBuffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            if (duration == TimeSpan.Zero)
            {
                return Empty<IList<TSource>>();
            }

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.TakeLastBuffer(observer, duration)));
        }

        public static IAsyncObservable<IList<TSource>> TakeLastBuffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            if (duration == TimeSpan.Zero)
            {
                return Empty<IList<TSource>>();
            }

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.TakeLastBuffer(observer, duration, scheduler)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> TakeLastBuffer<TSource>(IAsyncObserver<IList<TSource>> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var queue = new Queue<TSource>();

            return Create<TSource>(
                x =>
                {
                    queue.Enqueue(x);

                    if (queue.Count > count)
                    {
                        queue.Dequeue();
                    }

                    return Task.CompletedTask;
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(queue.ToList()).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> TakeLastBuffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan duration)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> TakeLastBuffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan duration, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }
    }
}
