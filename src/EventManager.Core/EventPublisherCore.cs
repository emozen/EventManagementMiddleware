﻿using EventManager.Core.Interfaces;
using EventManager.Shared.Dto;
using EventManager.Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace EventManager.Core
{
    /// <summary>
    /// this use TPublisher , inject it to dependency injection and initialize it before use call publish method
    /// inject this as a singleteon
    /// </summary>
    public class EventPublisherCore<TPublisher> : IEMPublisher where TPublisher : IMessagePublisher
    {
        IEventManagerCore _eventManager;
        TPublisher _genericPublisher;
        public EventPublisherCore(TPublisher genericPublisher, IEventManagerCore eventManager)
        {
            _genericPublisher = genericPublisher;
            _eventManager = eventManager;
        }

        public void Publish<T>(T eMEvent) where T : IEMEvent
        {
            EMEvent<T> nodeEvent;
            try
            {
                nodeEvent = _eventManager.GetEvent(eMEvent);
            }
            catch (Exception e)
            {
                throw new Exception("Error while event info. For more information see inner exception", e);
            }

            string msgStr = "";
            try
            {
                msgStr = Newtonsoft.Json.JsonConvert.SerializeObject(nodeEvent);
            }
            catch (Exception e)
            {
                throw new Exception("Error while serializing object to json. For more information see inner exception", e);
            }


            try
            {
                _genericPublisher.Publish(msgStr);
            }
            catch (Exception e)
            {
                throw new Exception("Error while publishing message. For more information see inner exception", e);
            }
        }

        public async Task PublishAsync<T>(T eMEvent) where T : IEMEvent
        {
            EMEvent<T> nodeEvent;
            try
            {
                nodeEvent = await _eventManager.GetEventAsync(eMEvent);
            }
            catch (Exception e)
            {
                throw new Exception("Error while event info. For more information see inner exception", e);
            }

            string msgStr = "";
            try
            {
                msgStr = Newtonsoft.Json.JsonConvert.SerializeObject(nodeEvent);
            }
            catch (Exception e)
            {
                throw new Exception("Error while serializing object to json. For more information see inner exception", e);
            }


            try
            {
                await _genericPublisher.PublishAsync(msgStr);
            }
            catch (Exception e)
            {
                throw new Exception("Error while publishing message. For more information see inner exception", e);
            }
        }

    }
}
