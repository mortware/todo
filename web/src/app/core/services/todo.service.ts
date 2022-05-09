import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { TodoItem } from '../models';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  static readonly BasePath = 'https://localhost:7050/todo';

  private refreshList$ = new BehaviorSubject<boolean>(true);

  list$: Observable<TodoItem[]>;

  constructor(private http: HttpClient) {

    this.list$ = this.list();

  }

  private list(): Observable<TodoItem[]> {
    return this.refreshList$.pipe(switchMap(
      _ => this.http.get<TodoItem[]>(TodoService.BasePath + '/list')
    ))
  }

  create(text: string): Observable<string> {
    return this.http.post<string>(TodoService.BasePath + '/create', { text }).pipe(
      tap(_ => {
        this.refreshList$.next(false);
      })
    );
  }

  setCompleted(id: string, dateTime: Date): Observable<TodoItem> {
    /* There's probably a better way of doing this rather than updating the entire list every time! */
    return this.http.patch<TodoItem>(`${TodoService.BasePath}/update`, { id: id, completed: dateTime }).pipe(
        tap(_ => {
          this.refreshList$.next(false);
        })
      );
  }
}
