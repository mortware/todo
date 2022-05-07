import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BehaviorSubject, catchError, map, Observable, throwError, tap } from 'rxjs';
import { TodoService } from 'src/app/core/services/todo.service';
import { BaseComponent } from '../base.component';
import { faCheck } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent extends BaseComponent {

  busySubject = new BehaviorSubject<boolean>(false);
  busy: boolean = false;
  vm$: Observable<any>;
  form: FormGroup;
  faCheck = faCheck;

  constructor(
    private todoService: TodoService,
    private formBuilder: FormBuilder) {
    super();

    this.form = this.formBuilder.group({
      text: new FormControl(null)
    })

    this.vm$ = this.todoService.list$.pipe(
      map(items => {
        return {
          items
        };
      }),
      catchError(err => {
        this.handleError('Failed to fetch items', err.message);
        return throwError(() => err);
      })
    );

    this.busySubject.pipe(
      tap(_ => {
        this.busy = _;
      })
    ).subscribe();
  }

  submit() {
    this.busySubject.next(true);
    this.todoService.create(this.form.get('text')?.value)
      .subscribe({
        complete: () => {
          this.busySubject.next(false);
          this.form.reset();
        },
        error: err => {
          this.handleError('Failed to create new item', err.message);
          return throwError(() => err);
        }
      });
  }
}
