import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BehaviorSubject, catchError, map, Observable, throwError, tap } from 'rxjs';
import { TodoService } from 'src/app/core/services/todo.service';
import { BaseComponent } from '../base.component';
import { faCheck, faXmark } from '@fortawesome/free-solid-svg-icons';
import { TodoItem } from 'src/app/core/models';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent extends BaseComponent {

  formBusySubject = new BehaviorSubject<boolean>(false);
  formBusy: boolean = false;
  vm$: Observable<any>;
  form: FormGroup;
  faCheck = faCheck;
  faXmark = faXmark;

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

    this.formBusySubject.pipe(
      tap(_ => {
        this.formBusy = _;
      })
    ).subscribe();
  }

  submit() {
    this.formBusySubject.next(true);
    this.todoService.create(this.form.get('text')?.value)
      .subscribe({
        complete: () => {
          this.formBusySubject.next(false);
          this.form.reset();
        },
        error: err => {
          this.handleError('Failed to create new item', err.message);
          return throwError(() => err);
        }
      });
  }

  setCompleted(item: TodoItem) {
    this.todoService.setCompleted(item.id, new Date())
      .subscribe({
        complete: () => {
          console.log("Completed call");
        },
        next: _ => {
          console.log(_);
          item = _;
        },
        error: err => {
          this.handleError('Failed to update existing item', err.message);
          return throwError(() => err);
        }
      });
  }
}
