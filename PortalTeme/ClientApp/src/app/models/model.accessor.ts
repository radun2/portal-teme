import { BaseModel } from './base.model';
import { CourseGroup } from './course.model';

export interface ModelAccessor {

    create(element: any): any;
    /**
     * This returns true if the item is newly added and the inputs must be shown.
     * @param item
     */
    isNew(item: any): boolean;
}

export class BaseModelAccessor implements ModelAccessor {
    isNew(item: BaseModel): boolean {
        return !item.id;
    }

    create(item: BaseModel): BaseModel {
        return {
            id: item.id || ''
        };
    }
}

export class CourseGroupModelAccessor implements ModelAccessor {
    isNew(item: CourseGroup): boolean {
        return !item.courseId && !item.groupId;
    }

    create(item: CourseGroup): CourseGroup {
        return {
            courseId: item.courseId || '',
            groupId: item.groupId || '',
            name: null
        };
    }
}
