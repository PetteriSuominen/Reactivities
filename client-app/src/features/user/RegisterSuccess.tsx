import React from 'react';
import queryString from "query-string";
import { Button, Header, Icon, Segment } from 'semantic-ui-react';
import agent from '../../app/api/agent';
import { toast } from 'react-toastify';
import { RouteComponentProps } from 'react-router-dom';

const RegisterSuccess: React.FC<RouteComponentProps> = ({ location }) => {
    const { email } = queryString.parse(location.search);

    const handleConfirmEmailResend = () => {
        agent.User.resendEmailConfirm(email as string)
            .then(() => {
                toast.success('Verification email resent - Please check your email');
            })
            .catch(error => {
                console.log(error);
            })
    }

    return (
        <Segment placeholder>
            <Header icon>
                <Icon name='check' />
                Successfully registered!
            </Header>
            <Segment.Inline>
                <div className='center'>
                    <p>Please check your email (including junk folder) for the verification email</p>
                    {email &&
                        <>
                            <p>Didn´t receive the email? Please click below button to resend</p>
                            <Button
                                primary
                                content="Resend email"
                                size="huge"
                                onClick={handleConfirmEmailResend}
                            />
                        </>
                    }
                </div>
            </Segment.Inline>
        </Segment>
    )
}

export default RegisterSuccess;
