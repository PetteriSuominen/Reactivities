export interface IProfile {
    displayName: string,
    username: string,
    bio: string,
    image: string,
    photos: IPhoto[]
};

export interface IPhoto {
    id: string,
    url: string,
    isMain: boolean
}

export interface IProfileFormValues {
    displayName: string;
    bio: string;
}

export class EditProfileFormValues implements IProfileFormValues {
    displayName: string = '';
    bio: string = '';

    constructor(init?: IProfile) {
        if (init) {
            this.displayName = init.displayName;
            this.bio = init.bio;
        }
    }
}