import React, { useContext } from 'react';
import { Form, Button } from 'semantic-ui-react';
import { Form as FinalForm, Field } from "react-final-form";
import TextInput from '../../../app/common/form/TextInput';
import TextAreaIput from '../../../app/common/form/TextAreaIput';
import { RootStoreContext } from '../../../app/stores/rootStore';
import { EditProfileFormValues, IProfileFormValues } from '../../../app/models/profile';
import { observer } from 'mobx-react-lite';

const ProfileEditForm = () => {
    const rootStore = useContext(RootStoreContext);
    const { profile, editProfile, submitting } = rootStore.profileStore;

    const formValues = profile ? new EditProfileFormValues(profile) : new EditProfileFormValues();

    return (
        <FinalForm
            onSubmit={(values: IProfileFormValues) => editProfile(values)}
            initialValues={formValues}
            render={({ handleSubmit, pristine }) => (
                <Form onSubmit={handleSubmit}>
                    <Field
                        name='displayName'
                        component={TextInput}
                    />
                    <Field
                        name='bio'
                        component={TextAreaIput}
                    />
                    <Button
                        loading={submitting}
                        disabled={pristine}
                        type='submit'
                        positive
                        floated='right'
                        content='Update profile' />
                </Form>
            )} />
    )
}

export default observer(ProfileEditForm);
