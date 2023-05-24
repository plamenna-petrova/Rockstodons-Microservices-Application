import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IStream } from '../interfaces/streams/stream';
import { Observable } from 'rxjs';
import { IStreamCreateDTO } from '../interfaces/streams/stream-create-dto';
import { IStreamUpdateDTO } from '../interfaces/streams/stream-update-dto';

@Injectable({
  providedIn: 'root'
})
export class StreamsService {
  private readonly streamsAPIUrl = `${environment.apiUrl}/streams`;

  constructor(private httpClient: HttpClient) {

  }

  getAllStreams(): Observable<IStream[]> {
    return this.httpClient.get<IStream[]>(this.streamsAPIUrl);
  }

  getStreamsWithFullDetails(): Observable<IStream[]> {
    return this.httpClient.get<IStream[]>(`${this.streamsAPIUrl}/all`);
  }

  getStreamById(streamId: string): Observable<IStream> {
    return this.httpClient.get<IStream>(`${this.streamsAPIUrl}/${streamId}`);
  }

  getStreamDetails(streamDetailsId: string): Observable<IStream> {
    return this.httpClient.get<IStream>(`${this.streamsAPIUrl}/details/${streamDetailsId}`);
  }

  createNewStream(streamToCreate: IStreamCreateDTO): Observable<any> {
    const mappedTracksForStreamCreation = streamToCreate.tracks
      .map(({ id, name }) => ({ id, name }));

    const mappedStreamToCreate = {
      name: streamToCreate.name,
      imageFileName: streamToCreate.imageFileName,
      imageUrl: streamToCreate.imageUrl,
      tracks: mappedTracksForStreamCreation
    };

    return this.httpClient.post<any>(`${this.streamsAPIUrl}/create`, mappedStreamToCreate);
  }

  updateStream(streamToUpdate: IStreamUpdateDTO): Observable<IStreamUpdateDTO> {
    const updateStreamRequestBody = (({ id, ...su }) => su)(streamToUpdate);
    return this.httpClient.put<IStreamUpdateDTO>(
      `${this.streamsAPIUrl}/update/${streamToUpdate.id}`, updateStreamRequestBody
    );
  }

  deleteStream(streamToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.streamsAPIUrl}/delete/${streamToDeleteId}`);
  }

  deleteStreamPermanently(streamToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.streamsAPIUrl}/confirm-deletion/${streamToDeletePermanentlyId}`
    );
  }

  restoreStream(streamToRestoreId: string): Observable<void> {
    return this.httpClient.post<void>(
      `${this.streamsAPIUrl}/restore/${streamToRestoreId}`, null
    )
  }
}
