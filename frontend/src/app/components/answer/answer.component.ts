import { LoadAnswerDetailsAction, LoadAnswerPreviewAction } from '../../store/answer/answer.actions';
import { AnswerState } from '../../store/answer/answer.reducer';
import { FormState } from '../../store/form/form.reducer';
import { AppState } from '../../store/store.module';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Rx';

@Component({
    templateUrl: './answer.component.html',
    styleUrls: ['./answer.component.scss']
})
export class AnswerComponent implements OnInit {

    answerState: Observable<AnswerState>;
    formState: Observable<FormState>;

    urgentHeader = "Ultimele sesizări trimise de observatorii pe care i-ai acreditat";
    urgentSubHeader = "Sunt marcate ca ugente pentru că reprezintă semnale de fraudă sau nereguli grave în secțiile în care au fost raportate.";
    notUrgentHeader = "Ultimele răspunsuri trimise de observatorii pe care i-ai acreditat";
    notUrgentSubheader = "Sunt listate toate răspunsurile date de fiecare observator, între care și cele de tip flagged, semnalate specific.";

    constructor(private store: Store<AppState>) { }

    ngOnInit() {
        this.formState = this.store.select(state => state.form).distinctUntilChanged();
        this.answerState = this.store.select(state => state.answer).distinctUntilChanged();
    }
    redoAnswerListAction() {
        // take the current state of the answerState, and do a reloaded
        this.store.select(state => state.answer).take(1)
            .map(s => new LoadAnswerPreviewAction(s.urgent, s.page, s.pageSize))
            .map(a => this.store.dispatch(a))
            .subscribe()
    }
    redoAnswerDetailsAction() {
        // take the current state of the answerState, and do a reloaded
        this.store.select(state => state.answer).take(1)
            .map(s => new LoadAnswerDetailsAction(s.observerId, s.sectionId))
            .map(a => this.store.dispatch(a))
            .subscribe();
    }

    pageChanged(event) {
        this.store.select(s => s.answer).take(1)
            .map(s => new LoadAnswerPreviewAction(s.urgent, event.page, event.pageSize))
            .map(a => this.store.dispatch(a))
            .subscribe();
    }

}
