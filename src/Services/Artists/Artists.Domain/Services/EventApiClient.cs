#region references

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artists.Domain.Application.Config;
using Artists.Domain.Application.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

#endregion

namespace Artists.Domain.Services {
    public interface IEventApiClient {
        #region Public Methods

        //Task<EventDTO> Get(
        //    Guid id
        //);

        //Task<EffortDTO> GetEffortById(
        //    Guid id
        //);

        //Task<List<EventDTO>> GetByConstituentId(
        //    Guid constituentId
        //);

        #endregion
    }

    public class EventApiClient : IEventApiClient {
        #region Private Fields

        private readonly ILogger<EventApiClient> _logger;

        private readonly IOptionsSnapshot<AppSettings> _settings;

        private readonly HttpClient _handler;

        #endregion

        #region Public Constructors

        public EventApiClient(
            ILogger<EventApiClient> logger,
            IOptionsSnapshot<AppSettings> settings,
            IHttpContextAccessor accessor,
            IETagCache cache
        ) {
            _logger = logger;
            _settings = settings;
            _handler = new HttpClient(new HttpClientAuthorizationDelegatingHandler(accessor, _settings, cache));
        }

        #endregion

        #region Public Methods

        //public async Task<EventDTO> Get(
        //    Guid id
        //) {
        //    _logger.LogInformation($"{nameof(Get)} executing for id {id}");

        //    string uri = GetByIdUri(_settings.Value.EventApiUri, id);

        //    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri);
        //    HttpResponseMessage response = await _handler.SendAsync(message, new CancellationToken());

        //    try
        //    {
        //        response.EnsureSuccessStatusCode();
        //        EventDTO dto = JsonConvert.DeserializeObject<EventDTO>(await response.Content.ReadAsStringAsync());
        //        return dto;
        //    }
        //    catch (HttpRequestException exc) {
        //        throw new OrderDomainException($"Fail to get event by id {id}", exc);
        //    }
        //}

        //public async Task<EffortDTO> GetEffortById(
        //    Guid id
        //) {
        //    _logger.LogInformation($"{nameof(GetEffortById)} executing for id {id}");

        //    string uri = GetEffortByIdUri(_settings.Value.EventApiUri, id);

        //    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri);
        //    HttpResponseMessage response = await _handler.SendAsync(message, new CancellationToken());

        //    try
        //    {
        //        response.EnsureSuccessStatusCode();
        //        EffortDTO dto =
        //            JsonConvert.DeserializeObject<EffortDTO>(await response.Content.ReadAsStringAsync());
        //        return dto;
        //    }
        //    catch (HttpRequestException exc) {
        //        throw new OrderDomainException($"Fail to get effort by id {id}", exc);
        //    }
        //}

        //public async Task<List<EventDTO>> GetByConstituentId(
        //    Guid constituentId
        //) {
        //    _logger.LogInformation($"{nameof(GetByConstituentId)} executing for constituentId {constituentId}");

        //    string uri = GetByConstituentIdUri(_settings.Value.EventApiUri, constituentId);

        //    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri);
        //    HttpResponseMessage response = await _handler.SendAsync(message, new CancellationToken());

        //    try
        //    {
        //        response.EnsureSuccessStatusCode();
        //        List<EventDTO> dto =
        //            JsonConvert.DeserializeObject<List<EventDTO>>(await response.Content.ReadAsStringAsync());
        //        return dto;
        //    }
        //    catch (HttpRequestException exc) {
        //        throw new OrderDomainException($"Fail to get events by constituent id {constituentId}", exc);
        //    }
        //}

        public string GetByIdUri(
            string baseUri,
            Guid id
        ) {
            //add slash at end if doesnt exist
            baseUri = baseUri.EndsWith("/") ? baseUri : baseUri + "/";

            return $"{baseUri}{Paths.EventsApiPath}/{id}";
        }

        public string GetByConstituentIdUri(
            string baseUri,
            Guid id
        ) {
            //add slash at end if doesnt exist
            baseUri = baseUri.EndsWith("/") ? baseUri : baseUri + "/";

            return $"{baseUri}{Paths.ConstituentsApiPath}/{id}/events";
        }

        public string GetEffortByIdUri(
            string baseUri,
            Guid id
        ) {
            //add slash at end if doesnt exist
            baseUri = baseUri.EndsWith("/") ? baseUri : baseUri + "/";

            return $"{baseUri}{Paths.EffortsApiPath}/{id}";
        }

        #endregion
    }
}