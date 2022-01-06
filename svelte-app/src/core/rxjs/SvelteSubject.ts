import {BehaviorSubject} from 'rxjs';

class SvelteSubject extends BehaviorSubject {
    set(value){
        super.next(value)
    }

    lift(operator) {
        const result = new SvelteSubject();
        result.operator = operator;
        result.source = this;
        return result;
    }
}

export default SvelteSubject;