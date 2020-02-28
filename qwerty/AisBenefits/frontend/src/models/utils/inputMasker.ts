export default class InputMasker {

    private minCaret: number;
    private maxCaret: number;

    private masks: Array<string | RegExp>;
    private filled: boolean;
    private placeholder: string;

    constructor(pattern: string, filled = true, placeholder = '_') {
        this.filled = filled;
        this.placeholder = placeholder;
        const parts = (pattern || '').split('');
        this.masks = [];
        let shieldNext = false;
        for (const part of parts) {
            if (shieldNext) {
                this.masks.push(part);
                shieldNext = false;
                continue;
            }
            switch (part) {
                case '\\':
                    shieldNext = true;
                    break;
                case '9':
                    this.masks.push(/[\d]/);
                    break;
                case 'h':
                    this.masks.push(/[0-9a-f]/);
                    break;
                case 'a':
                    this.masks.push(/[a-zA-Zа-яА-ЯёЁ]/);
                    break;
                case 'r':
                    this.masks.push(/[а-яА-ЯёЁ]/);
                    break;
                default:
                    this.masks.push(part);
            }
        }
        this.minCaret = this.masks.findIndex((el) => typeof el !== 'string');
        this.maxCaret = this.masks.length;
    }

    public input(e: KeyboardEvent) {
        if (!e || !e.target) {
            return '';
        }
        const target = e.target as HTMLInputElement;
        return this.mask(target.value || '');
    }

    public mask(value: string) {
        value = (value || '');
        const parts = value.split('', value.indexOf(this.placeholder));
        let result = '';
        let n = 0;
        for (let i = 0; i < this.masks.length; i++) {
            const mask = this.masks[i];
            let part = parts[n];
            if (typeof mask === 'string') {
                if (result.length !== i) {
                    break;
                }
                result += mask;
                if (part === mask) {
                    n++;
                }
                continue;
            }
            if (!part) {
                break;
            }
            if (mask.test) {
                if (part && mask.test(part)) {
                    result += part;
                } else {
                    while (part && !mask.test(part)) {
                        part = parts[++n];
                    }
                    result += part || '';
                }
                n++;
            }
        }
        this.maxCaret = result.length;
        if (this.filled) {
            while (this.masks[result.length]) {
                result += typeof this.masks[result.length] === 'string'
                    ? this.masks[result.length]
                    : this.placeholder;
            }
        }
        return result;
    }

    public validate(value: string): boolean {
        return this.mask(value).length === this.masks.length;
    }

    public getCaret(pos?: number, backward: boolean = false, clicked: boolean = false): number {
        if (pos === undefined) {
            return this.maxCaret;
        }
        let caret = Math.max(pos, this.minCaret);

        if (!clicked) {
            if (backward) {
                while (typeof this.masks[caret - 1] === 'string') {
                    caret--;
                }
            } else {
                const needInc = typeof this.masks[caret - 1] === 'string';
                while (typeof this.masks[caret] === 'string') {
                    caret++;
                }
                if (needInc) {
                    caret++;
                }
            }
        }
        return Math.max(this.minCaret, (this.maxCaret > 0 ? Math.min(caret, this.maxCaret) : caret));
    }
}
