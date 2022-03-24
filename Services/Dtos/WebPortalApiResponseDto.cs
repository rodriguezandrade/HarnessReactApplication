using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class WebPortalApiResponseDto
    {
        [JsonProperty("recordingPackages")]
        public List<RecordingPackage> RecordingPackages { get; set; }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("totalCases")]
        public int TotalCases { get; set; }

        [JsonProperty("isOrganizationUser")]
        public bool IsOrganizationUser { get; set; }
    }

    public class Audio
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("dateTimeStamp")]
        public DateTime DateTimeStamp { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("mediaType")]
        public string MediaType { get; set; }

        [JsonProperty("fullFilePath")]
        public string FullFilePath { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public class RecordingPackage
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("dateTimeStamp")]
        public DateTime DateTimeStamp { get; set; }

        [JsonProperty("caseCode")]
        public string CaseCode { get; set; }

        [JsonProperty("sessionName")]
        public string SessionName { get; set; }

        [JsonProperty("uniqueCaseNumber")]
        public string UniqueCaseNumber { get; set; }

        [JsonProperty("caseNumber")]
        public string CaseNumber { get; set; }

        [JsonProperty("caseDate")]
        public DateTime CaseDate { get; set; }

        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("division")]
        public string Division { get; set; }

        [JsonProperty("recordingType")]
        public string RecordingType { get; set; }

        [JsonProperty("roomName")]
        public string RoomName { get; set; }

        [JsonProperty("customProfileCode")]
        public object CustomProfileCode { get; set; }

        [JsonProperty("systemName")]
        public string SystemName { get; set; }

        [JsonProperty("submittedByUser")]
        public object SubmittedByUser { get; set; }

        [JsonProperty("matterTitle")]
        public object MatterTitle { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("audioDuration")]
        public int AudioDuration { get; set; }

        [JsonProperty("isRecordingFinished")]
        public bool IsRecordingFinished { get; set; }

        [JsonProperty("hasM4A")]
        public bool HasM4A { get; set; }

        [JsonProperty("m4ARequested")]
        public bool M4ARequested { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("people")]
        public List<object> People { get; set; }

        [JsonProperty("annotations")]
        public List<object> Annotations { get; set; }

        [JsonProperty("comments")]
        public List<object> Comments { get; set; }

        [JsonProperty("notes")]
        public List<object> Notes { get; set; }

        [JsonProperty("documents")]
        public List<object> Documents { get; set; }

        [JsonProperty("audios")]
        public List<Audio> Audios { get; set; }

        [JsonProperty("videos")]
        public List<object> Videos { get; set; }

        [JsonProperty("properties")]
        public List<object> Properties { get; set; }
    }
}
