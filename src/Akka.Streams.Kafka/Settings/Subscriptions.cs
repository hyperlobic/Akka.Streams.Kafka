using System.Collections.Immutable;
using Confluent.Kafka;

namespace Akka.Streams.Kafka.Settings
{
    public interface ISubscription { }
    public interface IManualSubscription : ISubscription { }
    public interface IAutoSubscription : ISubscription { }

    /// <summary>
    /// TopicSubscription
    /// </summary>
    internal sealed class TopicSubscription : IAutoSubscription
    {
        /// <summary>
        /// TopicSubscription
        /// </summary>
        /// <param name="topics">List of topics to subscribe</param>
        public TopicSubscription(IImmutableSet<string> topics)
        {
            Topics = topics;
        }

        /// <summary>
        /// List of topics to subscribe
        /// </summary>
        public IImmutableSet<string> Topics { get; }
    }
    
    /// <summary>
    /// TopicSubscriptionPattern
    /// </summary>
    /// <remarks>
    /// Allows subscription to multiple topics, matching given regex pattern
    /// </remarks>
    internal sealed class TopicSubscriptionPattern : IAutoSubscription
    {
        /// <summary>
        /// TopicSubscriptionPattern
        /// </summary>
        /// <param name="topicPattern">Topic pattern (regular expression to be matched)</param>
        public TopicSubscriptionPattern(string topicPattern)
        {
            TopicPattern = topicPattern;
        }

        /// <summary>
        /// Topic pattern (regular expression to be matched)
        /// </summary>
        public string TopicPattern { get; }
    }

    /// <summary>
    /// Assignment subscription
    /// </summary>
    /// <remarks>
    /// Allows to subscribe to fixed set of topic partitions
    /// </remarks>
    internal sealed class Assignment : IManualSubscription
    {
        /// <summary>
        /// Assignment
        /// </summary>
        /// <param name="topicPartitions">List of topic partitions to subscribe</param>
        public Assignment(IImmutableSet<TopicPartition> topicPartitions)
        {
            TopicPartitions = topicPartitions;
        }

        /// <summary>
        /// Topic partitions to subscribe
        /// </summary>
        public IImmutableSet<TopicPartition> TopicPartitions { get; }
    }

    /// <summary>
    /// Assignment with offset subscription
    /// </summary>
    /// <remarks>
    /// Allows to subscribe to fixed set of topic partitions with initial offsets specified
    /// </remarks>
    internal sealed class AssignmentWithOffset : IManualSubscription
    {
        /// <summary>
        /// AssignmentWithOffset
        /// </summary>
        /// <param name="topicPartitions">List of topic paritions with offsets to subscribe</param>
        public AssignmentWithOffset(IImmutableSet<TopicPartitionOffset> topicPartitions)
        {
            TopicPartitions = topicPartitions;
        }

        /// <summary>
        /// List of topic paritions with offsets to subscribe
        /// </summary>
        public IImmutableSet<TopicPartitionOffset> TopicPartitions { get; }
    }

    /// <summary>
    /// Subscriptions
    /// </summary>
    public static class Subscriptions
    {
        /// <summary>
        /// Generates <see cref="TopicSubscription"/>
        /// </summary>
        /// <param name="topics">Topics to subscribe</param>
        public static IAutoSubscription Topics(params string[] topics) =>
            new TopicSubscription(topics.ToImmutableHashSet());
        
        /// <summary>
        /// Generates <see cref="TopicSubscriptionPattern"/>
        /// </summary>
        /// <param name="topicPattern">Topic pattern</param>
        public static IAutoSubscription TopicPattern(string topicPattern) =>
            new TopicSubscriptionPattern(topicPattern);

        /// <summary>
        /// Generates <see cref="Assignment"/>
        /// </summary>
        /// <param name="topicPartitions">Topic parititions to subscribe</param>
        public static IManualSubscription Assignment(params TopicPartition[] topicPartitions) =>
            new Assignment(topicPartitions.ToImmutableHashSet());

        /// <summary>
        /// Generates <see cref="AssignmentWithOffset"/>
        /// </summary>
        /// <param name="topicPartitions">Topic parititions with offsets to subscribe</param>
        public static IManualSubscription AssignmentWithOffset(params TopicPartitionOffset[] topicPartitions) =>
            new AssignmentWithOffset(topicPartitions.ToImmutableHashSet());
    }
}